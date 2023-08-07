﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Collections;
using Npgsql;
using System.Linq;
using System.Collections.ObjectModel;

namespace demo_sdk_hikvision
{
    public partial class eventos : Form
    {
        
        public eventos()
        {
            InitializeComponent();
            this.Text = "Desde [" + Variables.evento_fecha_inicio + "] hasta [" + Variables.evento_fecha_fin + "] - [" + Variables.evento_tipo_principal + "][" + Variables.evento_tipo_secundario + "] - [" + Variables.direccion_ip + "] - Roanet";
            btn_ExportToPostgres.Enabled = false;
            cargar();
            btn_ExportToPostgres.Enabled = true;
            //Dar visibilidadd al boton
        }

        List<Dictionary<string,string>> Markings = new List<Dictionary<string,string>>();  
        

        private string CsTemp = null;
        private int m_lLogNum = 0;
        public int m_lGetAcsEventHandle = -1;
        Thread m_pDisplayListThread = null;
        DateTime evento_fecha_inicio = new DateTime();
        DateTime evento_fecha_fin = new DateTime();
        string evento_tipo_principal = "";
        string evento_tipo_secundario = "";

        private void cargar()
        {
            if(Dispositivo.conectar(Variables.direccion_ip, Convert.ToInt32(Variables.puerto), Variables.usuario, MD5Crypto.Desencriptar(Variables.contrasena)) == "OK")
            {
                descargar_eventos();
            }
            else
            {
                MessageBox.Show("Dispositivo fuera de linea", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            limpiar();
        }

        public void descargar_eventos()
        {
            evento_fecha_inicio = Convert.ToDateTime(Variables.evento_fecha_inicio);
            evento_fecha_fin = Convert.ToDateTime(Variables.evento_fecha_fin);
            evento_tipo_principal = Variables.evento_tipo_principal;
            evento_tipo_secundario = Variables.evento_tipo_secundario;

            listViewEvent.Items.Clear();
            m_lLogNum = 0;
            HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_COND struCond = new HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);

            struCond.dwMajor = GetAcsEventType.ReturnMajorTypeValue(ref evento_tipo_principal);
            struCond.dwMinor = GetAcsEventType.ReturnMinorTypeValue(ref evento_tipo_secundario);

            struCond.struStartTime.dwYear = evento_fecha_inicio.Year;
            struCond.struStartTime.dwMonth = evento_fecha_inicio.Month;
            struCond.struStartTime.dwDay = evento_fecha_inicio.Day;
            struCond.struStartTime.dwHour = evento_fecha_inicio.Hour;
            struCond.struStartTime.dwMinute = evento_fecha_inicio.Minute;
            struCond.struStartTime.dwSecond = evento_fecha_inicio.Second;

            struCond.struEndTime.dwYear = evento_fecha_fin.Year;
            struCond.struEndTime.dwMonth = evento_fecha_fin.Month;
            struCond.struEndTime.dwDay = evento_fecha_fin.Day;
            struCond.struEndTime.dwHour = evento_fecha_fin.Hour;
            struCond.struEndTime.dwMinute = evento_fecha_fin.Minute;
            struCond.struEndTime.dwSecond = evento_fecha_fin.Second;

            struCond.byPicEnable = 0;
            struCond.szMonitorID = "";
            struCond.wInductiveEventType = 65535;

            uint dwSize = struCond.dwSize;
            IntPtr ptrCond = Marshal.AllocHGlobal((int)dwSize);
            Marshal.StructureToPtr(struCond, ptrCond, false);
            m_lGetAcsEventHandle = HCNetSDK_EventoACS.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_EventoACS.NET_DVR_GET_ACS_EVENT, ptrCond, (int)dwSize, null, IntPtr.Zero);
            if (-1 == m_lGetAcsEventHandle)
            {
                Marshal.FreeHGlobal(ptrCond);
                MessageBox.Show("NET_DVR_StartRemoteConfig FAIL, ERROR CODE" + HCNetSDK_EventoACS.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            m_pDisplayListThread = new Thread(procesamiento_eventos);
            m_pDisplayListThread.Start();
            Marshal.FreeHGlobal(ptrCond);
        }

        public void procesamiento_eventos()
        {
            int dwStatus = 0;
            Boolean Flag = true;
            HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struCFG = new HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG();
            struCFG.dwSize = (uint)Marshal.SizeOf(struCFG);
            int dwOutBuffSize = (int)struCFG.dwSize;
            struCFG.init();
            while (Flag)
            {
                dwStatus = HCNetSDK_EventoACS.NET_DVR_GetNextRemoteConfig(m_lGetAcsEventHandle, ref struCFG, dwOutBuffSize);
                switch (dwStatus)
                {
                    case HCNetSDK_EventoACS.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        procesamiento_eventos_acs(ref struCFG, ref Flag);
                        break;
                    case HCNetSDK_EventoACS.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        Thread.Sleep(200);
                        break;
                    case HCNetSDK_EventoACS.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_EventoACS.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_EventoACS.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;
                    case HCNetSDK_EventoACS.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_EventoACS.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_EventoACS.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        HCNetSDK_EventoACS.NET_DVR_StopRemoteConfig(m_lGetAcsEventHandle);
                        break;
                }
                //MessageBox.Show("paso");
            }
            //MessageBox.Show("paso");
        }

        private void procesamiento_eventos_acs(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struCFG, ref bool flag)
        {
            try
            {
                mostrar_lista_tarjeta(struCFG);
            }
            catch
            {
                MessageBox.Show("AddAcsEventToList Failed", "Error", MessageBoxButtons.OK);
                flag = false;
            }
        }

        public void mostrar_lista_tarjeta(HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struCFG)
        {
            if (this.InvokeRequired)
            {
                Delegate delegateProc = new ShowCardListThread(agregar_eventos_acs_lista);
                this.BeginInvoke(delegateProc, struCFG);
            }
            else
            {
                agregar_eventos_acs_lista(struCFG);
            }
        }

        private void agregar_eventos_acs_lista(HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            var marking = new Dictionary<string, string>();

            marking.Add("ip", Variables.direccion_ip);
            //this.listViewEvent.BeginUpdate();
            ListViewItem Item = new ListViewItem();
            Item.Text = (++m_lLogNum).ToString();

            string LogTime = GetStrLogTime(ref struEventCfg.struTime);// Fecha
            Item.SubItems.Add(LogTime);
            marking.Add("fecha", LogTime);

            string Major = ProcessMajorType(ref struEventCfg.dwMajor);
            Item.SubItems.Add(Major);

            ProcessMinorType(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            CsTemp = System.Text.Encoding.UTF8.GetString(struEventCfg.struAcsEventInfo.byCardNo);//numero tarjeta
           // CsTemp = System.Text.Encoding.Unicode.GetString(struEventCfg.struAcsEventInfo.byCardNo);//numero tarjeta

            Item.SubItems.Add(CsTemp);
            marking.Add("n_tarjeta", CsTemp);

            CardTypeMap(ref struEventCfg);// Tipo tarjeta
            marking.Add("tipo_tarjeta", CsTemp);

            //MessageBox.Show("tipo tarjeta: "+CsTemp);
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byWhiteListNo.ToString());//WhiteList

            ProcessReportChannel(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessCardReader(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            CsTemp = struEventCfg.struAcsEventInfo.dwCardReaderNo.ToString();
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwDoorNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwVerifyNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwAlarmInNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwAlarmOutNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwCaseSensorNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwRs485No.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwMultiCardGroupNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.wAccessChannel.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byDeviceNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwEmployeeNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.byDistractControlNo.ToString());

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.wLocalControllerID.ToString());

            ProcessInternatAccess(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessByType(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessMacAdd(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            ProcessSwipeCard(ref struEventCfg);
            Item.SubItems.Add(CsTemp);

            Item.SubItems.Add(struEventCfg.struAcsEventInfo.dwSerialNo.ToString());

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerLampID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerIRAdaptorID.ToString()*/);

            Item.SubItems.Add("0"/*struEventCfg.struAcsEventInfo.byChannelControllerIREmitterID.ToString()*/);

            //if (struEventCfg.wInductiveEventType < (ushort)GetAcsEventType.NumOfInductiveEvent())
            //{
            Item.SubItems.Add("0"/*GetAcsEventType.FindKeyOfInductive(struEventCfg.wInductiveEventType)*/);
            //}
            //else
            //{
            //    Item.SubItems.Add("Invalid");
            //}

            Item.SubItems.Add("0");//RecordChannelNum

            //ProcessbyUserType(ref struEventCfg);
            Item.SubItems.Add("0");

            //ProcessVerifyMode(ref struEventCfg);
            Item.SubItems.Add("0");

            //CsTemp = System.Text.Encoding.UTF8.GetString(struEventCfg.struAcsEventInfo.byEmployeeNo);
            Item.SubItems.Add("0");

            CsTemp = null;
            this.listViewEvent.Items.Add(Item);
            //this.listViewEvent.EndUpdate();
            if (marking["tipo_tarjeta"] == "Ordinary Card")
            {
                string n_tarjeta = "";
                foreach (char caracter in marking["n_tarjeta"])
                {
                    if (caracter >= 48 & caracter <=57)
                    {
                        n_tarjeta += caracter;
                    }

                }
                marking["n_tarjeta"] = n_tarjeta;
                Markings.Add(marking);
            }
        }

        private void ProcessByType(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byType)
            {
                case 0:
                    CsTemp = "Instant Zone";
                    break;
                case 1:
                    CsTemp = "24 Hour Zone";
                    break;
                case 2:
                    CsTemp = "Delay Zone";
                    break;
                case 3:
                    CsTemp = "Internal Zone";
                    break;
                case 4:
                    CsTemp = "Key Zone";
                    break;
                case 5:
                    CsTemp = "Fire Zone";
                    break;
                case 6:
                    CsTemp = "Perimeter Zone";
                    break;
                case 7:
                    CsTemp = "24 Hour Silent Zone";
                    break;
                case 8:
                    CsTemp = "24 Hour Auxiliary Zone";
                    break;
                case 9:
                    CsTemp = "24 Hour Vibration Zone";
                    break;
                case 10:
                    CsTemp = "Acs Emergency Open Zone";
                    break;
                case 11:
                    CsTemp = "Acs Emergency Close Zone";
                    break;
                default:
                    CsTemp = "No Effect";
                    break;
            }
        }

        private void ProcessInternatAccess(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byInternetAccess)
            {
                case 1:
                    CsTemp = "Up Network Port 1";
                    break;
                case 2:
                    CsTemp = "Up Network Port 2";
                    break;
                case 3:
                    CsTemp = "Down Network Port 1";
                    break;
                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessSwipeCard(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            if (struEventCfg.struAcsEventInfo.bySwipeCardType == 1)
            {
                CsTemp = "QR Code";
            }
            else
            {
                CsTemp = "No effect";
            }
        }

        private void ProcessMacAdd(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            if (struEventCfg.struAcsEventInfo.byMACAddr[0] != 0)
            {
                CsTemp = struEventCfg.struAcsEventInfo.byMACAddr[0].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[1].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[2].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[3].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[4].ToString() + ":" +
                    struEventCfg.struAcsEventInfo.byMACAddr[5].ToString();
            }
            else
            {
                CsTemp = "No Effect";
            }
        }

        private void CardTypeMap(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byCardType)
            {
                case 1:
                    CsTemp = "Ordinary Card";
                    break;
                case 2:
                    CsTemp = "Disabled Card";
                    break;
                case 3:
                    CsTemp = "Black List Card";
                    break;
                case 4:
                    CsTemp = "Patrol Card";
                    break;
                case 5:
                    CsTemp = "Stress Card";
                    break;
                case 6:
                    CsTemp = "Super Card";
                    break;
                case 7:
                    CsTemp = "Guest Card";
                    break;
                case 8:
                    CsTemp = "Release Card";
                    break;
                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessReportChannel(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG EventCfg)
        {
            switch (EventCfg.struAcsEventInfo.byReportChannel)
            {
                case 1:
                    CsTemp = "Upload";
                    break;
                case 2:
                    CsTemp = "Center 1 Upload";
                    break;
                case 3:
                    CsTemp = "Center 2 Upload";
                    break;
                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private void ProcessCardReader(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.struAcsEventInfo.byCardReaderKind)
            {
                case 1:
                    CsTemp = "IC Reader";
                    break;
                case 2:
                    CsTemp = "Certificate Reader";
                    break;
                case 3:
                    CsTemp = "Two-dimension Reader";
                    break;
                case 4:
                    CsTemp = "Finger Print Head";
                    break;
                default:
                    CsTemp = "No effect";
                    break;
            }
        }

        private string GetStrLogTime(ref HCNetSDK_EventoACS.NET_DVR_TIME time)
        {
            string res = time.dwYear.ToString() + "/" + time.dwMonth.ToString() + "/"
                + time.dwDay.ToString() + " " + time.dwHour.ToString() + ":" + time.dwMinute.ToString()
                + ":" + time.dwSecond.ToString();
            return res;
        }

        private string ProcessMajorType(ref int dwMajor)
        {
            string res = null;
            switch (dwMajor)
            {
                case 1:
                    res = "Alarm";
                    break;
                case 2:
                    res = "Exception";
                    break;
                case 3:
                    res = "Operation";
                    break;
                case 5:
                    res = "Event";
                    break;
                default:
                    res = "Unknown";
                    break;
            }
            return res;
        }

        private string ProcessMajorType(ref uint dwMajor)
        {
            string res = null;
            switch (dwMajor)
            {
                case 1:
                    res = "Alarm";
                    break;
                case 2:
                    res = "Exception";
                    break;
                case 3:
                    res = "Operation";
                    break;
                case 5:
                    res = "Event";
                    break;
                default:
                    res = "Unknown";
                    break;
            }
            return res;
        }

        private void ProcessMinorType(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMajor)
            {
                case HCNetSDK_EventoACS.MAJOR_ALARM:
                    AlarmMinorTypeMap(ref struEventCfg);
                    break;
                case HCNetSDK_EventoACS.MAJOR_EXCEPTION:
                    ExceptionMinorTypeMap(ref struEventCfg);
                    break;
                case HCNetSDK_EventoACS.MAJOR_OPERATION:
                    OperationMinorTypeMap(ref struEventCfg);
                    break;
                case HCNetSDK_EventoACS.MAJOR_EVENT:
                    EventMinorTypeMap(ref struEventCfg);
                    break;
                default:
                    CsTemp = "Unknown";
                    break;
            }
        }

        private void AlarmMinorTypeMap(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case HCNetSDK_EventoACS.MINOR_ALARMIN_SHORT_CIRCUIT:
                    CsTemp = "ALARMIN_SHORT_CIRCUIT";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMIN_BROKEN_CIRCUIT:
                    CsTemp = "ALARMIN_BROKEN_CIRCUIT";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMIN_EXCEPTION:
                    CsTemp = "ALARMIN_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMIN_RESUME:
                    CsTemp = "ALARMIN_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_HOST_DESMANTLE_ALARM:
                    CsTemp = "HOST_DESMANTLE_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_HOST_DESMANTLE_RESUME:
                    CsTemp = "HOST_DESMANTLE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_READER_DESMANTLE_ALARM:
                    CsTemp = "CARD_READER_DESMANTLE_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_READER_DESMANTLE_RESUME:
                    CsTemp = "CARD_READER_DESMANTLE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CASE_SENSOR_ALARM:
                    CsTemp = "CASE_SENSOR_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_CASE_SENSOR_RESUME:
                    CsTemp = "CASE_SENSOR_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_STRESS_ALARM:
                    CsTemp = "STRESS_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_OFFLINE_ECENT_NEARLY_FULL:
                    CsTemp = "OFFLINE_ECENT_NEARLY_FULL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_MAX_AUTHENTICATE_FAIL:
                    CsTemp = "CARD_MAX_AUTHENTICATE_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_SD_CARD_FULL:
                    CsTemp = "MINOR_SD_CARD_FULL";
                    break;
                case HCNetSDK_EventoACS.MINOR_LINKAGE_CAPTURE_PIC:
                    CsTemp = "MINOR_LINKAGE_CAPTURE_PIC";
                    break;
                case HCNetSDK_EventoACS.MINOR_SECURITY_MODULE_DESMANTLE_ALARM:
                    CsTemp = "MINOR_SECURITY_MODULE_DESMANTLE_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_SECURITY_MODULE_DESMANTLE_RESUME:
                    CsTemp = "MINOR_SECURITY_MODULE_DESMANTLE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_POS_START_ALARM:
                    CsTemp = "MINOR_POS_START_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_POS_END_ALARM:
                    CsTemp = "MINOR_POS_END_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_FACE_IMAGE_QUALITY_LOW:
                    CsTemp = "MINOR_FACE_IMAGE_QUALITY_LOW";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGE_RPRINT_QUALITY_LOW:
                    CsTemp = "MINOR_FINGE_RPRINT_QUALITY_LOW";
                    break;
                case HCNetSDK_EventoACS.MINOR_FIRE_IMPORT_SHORT_CIRCUIT:
                    CsTemp = "MINOR_FIRE_IMPORT_SHORT_CIRCUIT";
                    break;
                case HCNetSDK_EventoACS.MINOR_FIRE_IMPORT_BROKEN_CIRCUIT:
                    CsTemp = "MINOR_FIRE_IMPORT_BROKEN_CIRCUIT";
                    break;
                case HCNetSDK_EventoACS.MINOR_FIRE_IMPORT_RESUME:
                    CsTemp = "MINOR_FIRE_IMPORT_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_FIRE_BUTTON_TRIGGER:
                    CsTemp = "FIRE_BUTTON_TRIGGER";
                    break;
                case HCNetSDK_EventoACS.MINOR_FIRE_BUTTON_RESUME:
                    CsTemp = "FIRE_BUTTON_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_MAINTENANCE_BUTTON_TRIGGER:
                    CsTemp = "MAINTENANCE_BUTTON_TRIGGER";
                    break;
                case HCNetSDK_EventoACS.MINOR_MAINTENANCE_BUTTON_RESUME:
                    CsTemp = "MAINTENANCE_BUTTON_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_EMERGENCY_BUTTON_TRIGGER:
                    CsTemp = "EMERGENCY_BUTTON_TRIGGER";
                    break;
                case HCNetSDK_EventoACS.MINOR_EMERGENCY_BUTTON_RESUME:
                    CsTemp = "EMERGENCY_BUTTON_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_DISTRACT_CONTROLLER_ALARM:
                    CsTemp = "DISTRACT_CONTROLLER_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_DISTRACT_CONTROLLER_RESUME:
                    CsTemp = "DISTRACT_CONTROLLER_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME:
                    CsTemp = "MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LEGAL_EVENT_NEARLY_FULL:
                    CsTemp = "MINOR_LEGAL_EVENT_NEARLY_FULL";
                    break;
                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void ExceptionMinorTypeMap(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case HCNetSDK_EventoACS.MINOR_NET_BROKEN:
                    CsTemp = "NET_BROKEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_RS485_DEVICE_ABNORMAL:
                    CsTemp = "RS485_DEVICE_ABNORMAL";
                    break;
                case HCNetSDK_EventoACS.MINOR_RS485_DEVICE_REVERT:
                    CsTemp = "RS485_DEVICE_REVERT";
                    break;
                case HCNetSDK_EventoACS.MINOR_DEV_POWER_ON:
                    CsTemp = "DEV_POWER_ON";
                    break;
                case HCNetSDK_EventoACS.MINOR_DEV_POWER_OFF:
                    CsTemp = "DEV_POWER_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_WATCH_DOG_RESET:
                    CsTemp = "WATCH_DOG_RESET";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOW_BATTERY:
                    CsTemp = "LOW_BATTERY";
                    break;
                case HCNetSDK_EventoACS.MINOR_BATTERY_RESUME:
                    CsTemp = "BATTERY_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_AC_OFF:
                    CsTemp = "AC_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_AC_RESUME:
                    CsTemp = "AC_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_NET_RESUME:
                    CsTemp = "NET_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_FLASH_ABNORMAL:
                    CsTemp = "FLASH_ABNORMAL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_READER_OFFLINE:
                    CsTemp = "CARD_READER_OFFLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_READER_RESUME:
                    CsTemp = "CARD_READER_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_INDICATOR_LIGHT_OFF:
                    CsTemp = "INDICATOR_LIGHT_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_INDICATOR_LIGHT_RESUME:
                    CsTemp = "INDICATOR_LIGHT_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_OFF:
                    CsTemp = "CHANNEL_CONTROLLER_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_CHANNEL_CONTROLLER_RESUME:
                    CsTemp = "CHANNEL_CONTROLLER_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_SECURITY_MODULE_OFF:
                    CsTemp = "SECURITY_MODULE_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_SECURITY_MODULE_RESUME:
                    CsTemp = "SECURITY_MODULE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_BATTERY_ELECTRIC_LOW:
                    CsTemp = "BATTERY_ELECTRIC_LOW";
                    break;
                case HCNetSDK_EventoACS.MINOR_BATTERY_ELECTRIC_RESUME:
                    CsTemp = "BATTERY_ELECTRIC_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_CONTROL_NET_BROKEN:
                    CsTemp = "LOCAL_CONTROL_NET_BROKEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_CONTROL_NET_RSUME:
                    CsTemp = "LOCAL_CONTROL_NET_RSUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_MASTER_RS485_LOOPNODE_BROKEN:
                    CsTemp = "MASTER_RS485_LOOPNODE_BROKEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_MASTER_RS485_LOOPNODE_RESUME:
                    CsTemp = "MASTER_RS485_LOOPNODE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_CONTROL_OFFLINE:
                    CsTemp = "LOCAL_CONTROL_OFFLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_CONTROL_RESUME:
                    CsTemp = "LOCAL_CONTROL_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN:
                    CsTemp = "LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME:
                    CsTemp = "LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_DISTRACT_CONTROLLER_ONLINE:
                    CsTemp = "DISTRACT_CONTROLLER_ONLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_DISTRACT_CONTROLLER_OFFLINE:
                    CsTemp = "DISTRACT_CONTROLLER_OFFLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_ID_CARD_READER_NOT_CONNECT:
                    CsTemp = "ID_CARD_READER_NOT_CONNECT";
                    break;
                case HCNetSDK_EventoACS.MINOR_ID_CARD_READER_RESUME:
                    CsTemp = "ID_CARD_READER_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGER_PRINT_MODULE_NOT_CONNECT:
                    CsTemp = "FINGER_PRINT_MODULE_NOT_CONNECT";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGER_PRINT_MODULE_RESUME:
                    CsTemp = "FINGER_PRINT_MODULE_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CAMERA_NOT_CONNECT:
                    CsTemp = "CAMERA_NOT_CONNECT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CAMERA_RESUME:
                    CsTemp = "CAMERA_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_COM_NOT_CONNECT:
                    CsTemp = "COM_NOT_CONNECT";
                    break;
                case HCNetSDK_EventoACS.MINOR_COM_RESUME:
                    CsTemp = "COM_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_DEVICE_NOT_AUTHORIZE:
                    CsTemp = "DEVICE_NOT_AUTHORIZE";
                    break;
                case HCNetSDK_EventoACS.MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE:
                    CsTemp = "PEOPLE_AND_ID_CARD_DEVICE_ONLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE:
                    CsTemp = "PEOPLE_AND_ID_CARD_DEVICE_OFFLINE";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_LOGIN_LOCK:
                    CsTemp = "LOCAL_LOGIN_LOCK";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_LOGIN_UNLOCK:
                    CsTemp = "LOCAL_LOGIN_UNLOCK";
                    break;
                case HCNetSDK_EventoACS.MINOR_SUBMARINEBACK_COMM_BREAK:
                    CsTemp = "SUBMARINEBACK_COMM_BREAK";
                    break;
                case HCNetSDK_EventoACS.MINOR_SUBMARINEBACK_COMM_RESUME:
                    CsTemp = "SUBMARINEBACK_COMM_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_MOTOR_SENSOR_EXCEPTION:
                    CsTemp = "MOTOR_SENSOR_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_CAN_BUS_EXCEPTION:
                    CsTemp = "CAN_BUS_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_CAN_BUS_RESUME:
                    CsTemp = "CAN_BUS_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_GATE_TEMPERATURE_OVERRUN:
                    CsTemp = "GATE_TEMPERATURE_OVERRUN";
                    break;
                case HCNetSDK_EventoACS.MINOR_IR_EMITTER_EXCEPTION:
                    CsTemp = "IR_EMITTER_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_IR_EMITTER_RESUME:
                    CsTemp = "IR_EMITTER_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LAMP_BOARD_COMM_EXCEPTION:
                    CsTemp = "LAMP_BOARD_COMM_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_LAMP_BOARD_COMM_RESUME:
                    CsTemp = "LAMP_BOARD_COMM_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_IR_ADAPTOR_COMM_EXCEPTION:
                    CsTemp = "IR_ADAPTOR_COMM_EXCEPTION";
                    break;
                case HCNetSDK_EventoACS.MINOR_IR_ADAPTOR_COMM_RESUME:
                    CsTemp = "IR_ADAPTOR_COMM_RESUME";
                    break;
                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void OperationMinorTypeMap(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case HCNetSDK_EventoACS.MINOR_LOCAL_UPGRADE:
                    CsTemp = "LOCAL_UPGRADE";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_LOGIN:
                    CsTemp = "REMOTE_LOGIN";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_LOGOUT:
                    CsTemp = "REMOTE_LOGOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ARM:
                    CsTemp = "REMOTE_ARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_DISARM:
                    CsTemp = "REMOTE_DISARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_REBOOT:
                    CsTemp = "REMOTE_REBOOT";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_UPGRADE:
                    CsTemp = "REMOTE_UPGRADE";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CFGFILE_OUTPUT:
                    CsTemp = "REMOTE_CFGFILE_OUTPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CFGFILE_INTPUT:
                    CsTemp = "REMOTE_CFGFILE_INTPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ALARMOUT_OPEN_MAN:
                    CsTemp = "REMOTE_ALARMOUT_OPEN_MAN";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ALARMOUT_CLOSE_MAN:
                    CsTemp = "REMOTE_ALARMOUT_CLOSE_MAN";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_OPEN_DOOR:
                    CsTemp = "REMOTE_OPEN_DOOR";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CLOSE_DOOR:
                    CsTemp = "REMOTE_CLOSE_DOOR";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ALWAYS_OPEN:
                    CsTemp = "REMOTE_ALWAYS_OPEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ALWAYS_CLOSE:
                    CsTemp = "REMOTE_ALWAYS_CLOSE";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CHECK_TIME:
                    CsTemp = "REMOTE_CHECK_TIME";
                    break;
                case HCNetSDK_EventoACS.MINOR_NTP_CHECK_TIME:
                    CsTemp = "NTP_CHECK_TIME";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CLEAR_CARD:
                    CsTemp = "REMOTE_CLEAR_CARD";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_RESTORE_CFG:
                    CsTemp = "REMOTE_RESTORE_CFG";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMIN_ARM:
                    CsTemp = "ALARMIN_ARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMIN_DISARM:
                    CsTemp = "ALARMIN_DISARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_RESTORE_CFG:
                    CsTemp = "LOCAL_RESTORE_CFG";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CAPTURE_PIC:
                    CsTemp = "REMOTE_CAPTURE_PIC";
                    break;
                case HCNetSDK_EventoACS.MINOR_MOD_NET_REPORT_CFG:
                    CsTemp = "MOD_NET_REPORT_CFG";
                    break;
                case HCNetSDK_EventoACS.MINOR_MOD_GPRS_REPORT_PARAM:
                    CsTemp = "MOD_GPRS_REPORT_PARAM";
                    break;
                case HCNetSDK_EventoACS.MINOR_MOD_REPORT_GROUP_PARAM:
                    CsTemp = "MOD_REPORT_GROUP_PARAM";
                    break;
                case HCNetSDK_EventoACS.MINOR_UNLOCK_PASSWORD_OPEN_DOOR:
                    CsTemp = "UNLOCK_PASSWORD_OPEN_DOOR";
                    break;
                case HCNetSDK_EventoACS.MINOR_AUTO_RENUMBER:
                    CsTemp = "AUTO_RENUMBER";
                    break;
                case HCNetSDK_EventoACS.MINOR_AUTO_COMPLEMENT_NUMBER:
                    CsTemp = "AUTO_COMPLEMENT_NUMBER";
                    break;
                case HCNetSDK_EventoACS.MINOR_NORMAL_CFGFILE_INPUT:
                    CsTemp = "NORMAL_CFGFILE_INPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_NORMAL_CFGFILE_OUTTPUT:
                    CsTemp = "NORMAL_CFGFILE_OUTTPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_RIGHT_INPUT:
                    CsTemp = "CARD_RIGHT_INPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_RIGHT_OUTTPUT:
                    CsTemp = "CARD_RIGHT_OUTTPUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_USB_UPGRADE:
                    CsTemp = "LOCAL_USB_UPGRADE";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_VISITOR_CALL_LADDER:
                    CsTemp = "REMOTE_VISITOR_CALL_LADDER";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_HOUSEHOLD_CALL_LADDER:
                    CsTemp = "REMOTE_HOUSEHOLD_CALL_LADDER";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ACTUAL_GUARD:
                    CsTemp = "REMOTE_ACTUAL_GUARD";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_ACTUAL_UNGUARD:
                    CsTemp = "REMOTE_ACTUAL_UNGUARD";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CONTROL_NOT_CODE_OPER_FAILED:
                    CsTemp = "REMOTE_CONTROL_NOT_CODE_OPER_FAILED";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CONTROL_CLOSE_DOOR:
                    CsTemp = "REMOTE_CONTROL_CLOSE_DOOR";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CONTROL_OPEN_DOOR:
                    CsTemp = "REMOTE_CONTROL_OPEN_DOOR";
                    break;
                case HCNetSDK_EventoACS.MINOR_REMOTE_CONTROL_ALWAYS_OPEN_DOOR:
                    CsTemp = "REMOTE_CONTROL_ALWAYS_OPEN_DOOR";
                    break;
                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        private void EventMinorTypeMap(ref HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struEventCfg)
        {
            switch (struEventCfg.dwMinor)
            {
                case HCNetSDK_EventoACS.MINOR_LEGAL_CARD_PASS:
                    CsTemp = "LEGAL_CARD_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_AND_PSW_PASS:
                    CsTemp = "CARD_AND_PSW_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_AND_PSW_FAIL:
                    CsTemp = "CARD_AND_PSW_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_AND_PSW_TIMEOUT:
                    CsTemp = "CARD_AND_PSW_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_AND_PSW_OVER_TIME:
                    CsTemp = "CARD_AND_PSW_OVER_TIME";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_NO_RIGHT:
                    CsTemp = "CARD_NO_RIGHT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_INVALID_PERIOD:
                    CsTemp = "CARD_INVALID_PERIOD";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_OUT_OF_DATE:
                    CsTemp = "CARD_OUT_OF_DATE";
                    break;
                case HCNetSDK_EventoACS.MINOR_INVALID_CARD:
                    CsTemp = "INVALID_CARD";
                    break;
                case HCNetSDK_EventoACS.MINOR_ANTI_SNEAK_FAIL:
                    CsTemp = "ANTI_SNEAK_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_INTERLOCK_DOOR_NOT_CLOSE:
                    CsTemp = "INTERLOCK_DOOR_NOT_CLOSE";
                    break;
                case HCNetSDK_EventoACS.MINOR_NOT_BELONG_MULTI_GROUP:
                    CsTemp = "NOT_BELONG_MULTI_GROUP";
                    break;
                case HCNetSDK_EventoACS.MINOR_INVALID_MULTI_VERIFY_PERIOD:
                    CsTemp = "INVALID_MULTI_VERIFY_PERIOD";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL:
                    CsTemp = "MULTI_VERIFY_SUPER_RIGHT_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL:
                    CsTemp = "MULTI_VERIFY_REMOTE_RIGHT_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_SUCCESS:
                    CsTemp = "MULTI_VERIFY_SUCCESS";
                    break;
                case HCNetSDK_EventoACS.MINOR_LEADER_CARD_OPEN_BEGIN:
                    CsTemp = "LEADER_CARD_OPEN_BEGIN";
                    break;
                case HCNetSDK_EventoACS.MINOR_LEADER_CARD_OPEN_END:
                    CsTemp = "LEADER_CARD_OPEN_END";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALWAYS_OPEN_BEGIN:
                    CsTemp = "ALWAYS_OPEN_BEGIN";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALWAYS_OPEN_END:
                    CsTemp = "ALWAYS_OPEN_END";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCK_OPEN:
                    CsTemp = "LOCK_OPEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCK_CLOSE:
                    CsTemp = "LOCK_CLOSE";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_BUTTON_PRESS:
                    CsTemp = "DOOR_BUTTON_PRESS";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_BUTTON_RELEASE:
                    CsTemp = "DOOR_BUTTON_RELEASE";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_NORMAL:
                    CsTemp = "DOOR_OPEN_NORMAL";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_CLOSE_NORMAL:
                    CsTemp = "DOOR_CLOSE_NORMAL";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_ABNORMAL:
                    CsTemp = "DOOR_OPEN_ABNORMAL";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_TIMEOUT:
                    CsTemp = "DOOR_OPEN_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMOUT_ON:
                    CsTemp = "ALARMOUT_ON";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALARMOUT_OFF:
                    CsTemp = "ALARMOUT_OFF";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALWAYS_CLOSE_BEGIN:
                    CsTemp = "ALWAYS_CLOSE_BEGIN";
                    break;
                case HCNetSDK_EventoACS.MINOR_ALWAYS_CLOSE_END:
                    CsTemp = "ALWAYS_CLOSE_END";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN:
                    CsTemp = "MULTI_VERIFY_NEED_REMOTE_OPEN";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS:
                    CsTemp = "MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_REPEAT_VERIFY:
                    CsTemp = "MULTI_VERIFY_REPEAT_VERIFY";
                    break;
                case HCNetSDK_EventoACS.MINOR_MULTI_VERIFY_TIMEOUT:
                    CsTemp = "MULTI_VERIFY_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOORBELL_RINGING:
                    CsTemp = "DOORBELL_RINGING";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_COMPARE_PASS:
                    CsTemp = "FINGERPRINT_COMPARE_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_COMPARE_FAIL:
                    CsTemp = "FINGERPRINT_COMPARE_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_VERIFY_PASS:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_VERIFY_FAIL:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT:
                    CsTemp = "CARD_FINGERPRINT_VERIFY_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    CsTemp = "CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_PASSWD_VERIFY_PASS:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    CsTemp = "FINGERPRINT_PASSWD_VERIFY_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_FINGERPRINT_INEXISTENCE:
                    CsTemp = "FINGERPRINT_INEXISTENCE";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_PLATFORM_VERIFY:
                    CsTemp = "CARD_PLATFORM_VERIFY";
                    break;
                case HCNetSDK_EventoACS.MINOR_MAC_DETECT:
                    CsTemp = "MINOR_MAC_DETECT";
                    break;
                case HCNetSDK_EventoACS.MINOR_LEGAL_MESSAGE:
                    CsTemp = "MINOR_LEGAL_MESSAGE";
                    break;
                case HCNetSDK_EventoACS.MINOR_ILLEGAL_MESSAGE:
                    CsTemp = "MINOR_ILLEGAL_MESSAGE";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_OR_DORMANT_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_AUTH_PLAN_DORMANT_FAIL:
                    CsTemp = "AUTH_PLAN_DORMANT_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_CARD_ENCRYPT_VERIFY_FAIL:
                    CsTemp = "CARD_ENCRYPT_VERIFY_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_SUBMARINEBACK_REPLY_FAIL:
                    CsTemp = "SUBMARINEBACK_REPLY_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_OR_DORMANT_OPEN_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_OPEN_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL:
                    CsTemp = "DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_TRAILING:
                    CsTemp = "TRAILING";
                    break;
                case HCNetSDK_EventoACS.MINOR_REVERSE_ACCESS:
                    CsTemp = "REVERSE_ACCESS";
                    break;
                case HCNetSDK_EventoACS.MINOR_FORCE_ACCESS:
                    CsTemp = "FORCE_ACCESS";
                    break;
                case HCNetSDK_EventoACS.MINOR_CLIMBING_OVER_GATE:
                    CsTemp = "CLIMBING_OVER_GATE";
                    break;
                case HCNetSDK_EventoACS.MINOR_PASSING_TIMEOUT:
                    CsTemp = "PASSING_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_INTRUSION_ALARM:
                    CsTemp = "INTRUSION_ALARM";
                    break;
                case HCNetSDK_EventoACS.MINOR_FREE_GATE_PASS_NOT_AUTH:
                    CsTemp = "FREE_GATE_PASS_NOT_AUTH";
                    break;
                case HCNetSDK_EventoACS.MINOR_DROP_ARM_BLOCK:
                    CsTemp = "DROP_ARM_BLOCK";
                    break;
                case HCNetSDK_EventoACS.MINOR_DROP_ARM_BLOCK_RESUME:
                    CsTemp = "DROP_ARM_BLOCK_RESUME";
                    break;
                case HCNetSDK_EventoACS.MINOR_LOCAL_FACE_MODELING_FAIL:
                    CsTemp = "LOCAL_FACE_MODELING_FAIL";
                    break;
                case HCNetSDK_EventoACS.MINOR_STAY_EVENT:
                    CsTemp = "STAY_EVENT";
                    break;
                case HCNetSDK_EventoACS.MINOR_PASSWORD_MISMATCH:
                    CsTemp = "PASSWORD_MISMATCH";
                    break;
                case HCNetSDK_EventoACS.MINOR_EMPLOYEE_NO_NOT_EXIST:
                    CsTemp = "EMPLOYEE_NO_NOT_EXIST";
                    break;
                case HCNetSDK_EventoACS.MINOR_COMBINED_VERIFY_PASS:
                    CsTemp = "COMBINED_VERIFY_PASS";
                    break;
                case HCNetSDK_EventoACS.MINOR_COMBINED_VERIFY_TIMEOUT:
                    CsTemp = "COMBINED_VERIFY_TIMEOUT";
                    break;
                case HCNetSDK_EventoACS.MINOR_VERIFY_MODE_MISMATCH:
                    CsTemp = "VERIFY_MODE_MISMATCH";
                    break;
                default:
                    CsTemp = Convert.ToString(struEventCfg.dwMinor, 16);
                    break;
            }
        }

        public delegate void ShowCardListThread(HCNetSDK_EventoACS.NET_DVR_ACS_EVENT_CFG struCFG);

        private static void limpiar()
        {
            Variables.evento_fecha_inicio = "";
            Variables.evento_fecha_fin = "";
            Variables.evento_tipo_principal = "";
            Variables.evento_tipo_secundario = "";
            //Variables.direccion_ip = "";
            Variables.puerto = 0;
            Variables.usuario = "";
            Variables.contrasena = "";

            //Variables.m_UserID = -1; Comentado por Lera
        }

        private void listViewEvent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_ExportToPostgres_Click(object sender, EventArgs e)
        {     
            string query = "";
            ConnectionPostgres Obj_ConnectionPg = new ConnectionPostgres();
            Obj_ConnectionPg.SetConnetion();
            foreach (var dic in Markings)
            {
               query = $"insert into h_marcaciones (dispositivo, n_tarjeta, fecha_marcacion,estado,fecha_grab)values" +
                    $"('{dic["ip"]}','{dic["n_tarjeta"]}','{dic["fecha"]}','a','{DateTime.Now.ToString(new CultureInfo("ja-JP"))}');";
                Obj_ConnectionPg.Insert(query);
            }
            Obj_ConnectionPg.CloseConnection();
            btn_ExportToPostgres.Enabled = false;
            MessageBox.Show("Se agregarón las nuevas marcaciones con éxito");
        }
    }
}
