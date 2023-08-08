using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace demo_sdk_hikvision
{
    public partial class Form1 : Form
    {
        ConnectionPostgres ObjConnectionPg = new ConnectionPostgres();
        public Form1()
        {
            InitializeComponent();

            if (HCNetSDK_Tarjeta.NET_DVR_Init() == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            HCNetSDK_Tarjeta.NET_DVR_SetLogToFile(3, "./Logs/", true);

            Set_dropdowns();
            mostrar_dispositivos();// ready
            mostrar_log();// ready
            
        }


        // ******************** mis variables
        //DataTable tabla_dispositivos_pg = new DataTable();
        string query = "";
        DataTable table_dropdown = new DataTable();
        //---------------------------------------------- VARIABLES 
        DataTable tabla_dispositivos = new DataTable();
        DataTable tabla_usuarios = new DataTable();
        //DataTable tabla_log = new DataTable();

        string id_tabla_dispositivos = "";
        string id_tabla_usuarios = "";
        string url_imagen = "";
        string url_huella = "";
        int tiempo_timer_tarjeta = 0;
        public Int32 m_lGetCardCfgHandle = -1;
        public Int32 m_lSetCardCfgHandle = -1;
        public Int32 m_lDelCardCfgHandle = -1;
        private int m_lGetFaceCfgHandle = -1;
        private int m_lSetFaceCfgHandle = -1;
        private int m_lCapFaceCfgHandle = -1;
        public int m_lGetFingerPrintCfgHandle = -1;
        public int m_lSetFingerPrintCfgHandle = -1;
        public int m_lDelFingerPrintCfHandle = -1;
        public int m_lCapFingerPrintCfHandle = -1;

        private HCNetSDK_Evento.MSGCallBack m_falarmData = null;

        //---------------------------------------------- MÉTODOS

        void Set_dropdowns()
        {   
            // devices
            query = $"select codigo, nombre from pdvs where estado='a'";
            ObjConnectionPg.SetConnetion();
            table_dropdown = ObjConnectionPg.Select(query);
            ObjConnectionPg.CloseConnection();

            foreach (DataRow row in table_dropdown.Rows)
            {
                Variables.DropdownItems_pdv.Add(row["nombre"].ToString() + " - " + row["codigo"].ToString());
            }         
        }
        void limpiar_dispositivos()
        {
            id_tabla_dispositivos = "";
            DropdownItems_pdv.Text = "";
            txt_disp_direcc_ip.Text = "";
            txt_disp_usuario.Text = "";
            txt_disp_contrasena.Text = "";
            txt_disp_puerto.Text = "8000";
            btn_disp_eliminar.Enabled = false;
        }

        private void mostrar_dispositivos()
        {
            string mensaje = "";
            data_dispositivos.Rows.Clear();
            query = $"select * from h_dispositivos order by direccion_ip";

            ObjConnectionPg.SetConnetion();
            tabla_dispositivos = ObjConnectionPg.Select(query);
            ObjConnectionPg.CloseConnection();

            for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
            {
                DataRow row = tabla_dispositivos.Rows[x];
                if (row["estatus"].ToString() == "True")
                {
                    mensaje = "En linea";
                }
                else
                {
                    mensaje = "Fuera de linea";
                }

                data_dispositivos.Rows.Add(row["id"], row["descripcion"], row["pdv"],row["direccion_ip"], row["puerto"], row["usuario"], row["contrasena"], mensaje);
            }
            id_tabla_dispositivos = "";

        }

        private void mostrar_log()
        {
            //data_user_add.Rows.Clear();
            //tabla_log = Consultas.Seleccionar_datatable("select * from log order by id desc");

            //query = $"select * from h_log order by id desc";
            //tabla_log = ConnectionBDPostgres.Select_Datatable(query,Obj_ConnectionPg.conex);

            //for (int x = 0; x < tabla_log.Rows.Count; x++)
            //{
            //    DataRow row = tabla_log.Rows[x];
            //    data_user_add.Rows.Add(row["fecha"],row["descripcion"]);
            //}
        }

        /// <summary>
        /// realiza la busqueda en la base de datos, de los empleados o usuarios de 
        /// acuerdo al parametro ingresado  y despliega el resultado de la busqueda en el DataGridView
        /// </summary>
        private void mostrar_usuarios()
        {          
            DataGridView_Usuarios.Rows.Clear();
            query = $"select conse,identificacion,nombre,pdv,huella from empleados where nombre ilike '%{txt_search_user.Text}%' or identificacion::text ilike '%{txt_search_user.Text}%';";
            ObjConnectionPg.SetConnetion();
            tabla_usuarios = ObjConnectionPg.Select(query);
            ObjConnectionPg.CloseConnection();

            foreach (DataRow row in tabla_usuarios.Rows)
            {
                DataGridView_Usuarios.Rows.Add(row["conse"],row["conse"], row["nombre"], row["identificacion"], row["pdv"], row["huella"]);

            }
            id_tabla_usuarios = "";
            tabla_usuarios.Rows.Clear ();
        }
        private bool Check_user_in_list()
        {
            DataGridViewRow CurrentRow = DataGridView_Usuarios.CurrentRow; //DataGridViewRow();
            foreach (DataGridViewRow row in data_user_add.Rows)
            {  
                if (CurrentRow.Cells["no_tarjeta"].Value.ToString() == row.Cells["no_tarjeta_add"].Value.ToString())
                {
                    MessageBox.Show("El empleado ya esta agregado a la lista", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return true;
                }
            }
            return false;
        }
        public static bool ping(string _direccion_ip, int tiempo_espera)
        {
            Ping HacerPing = new Ping();
            int iTiempoEspera = tiempo_espera;
            PingReply RespuestaPing;
            string sDireccion = _direccion_ip;
            RespuestaPing = HacerPing.Send(sDireccion, iTiempoEspera);
            if (RespuestaPing.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void limpiar_usuarios()
        {
            //txt_usuario_nombre.Text = "";
            //txt_usuario_no_tarjeta.Text = "";
            //txt_usuario_niv_acceso.Text = "1";
            //txt_usuario_cedula.Text = "";
            btn_usuario_eliminar.Enabled = true;
            url_imagen = "";
            url_huella = "";
            //btn_usuario_actualizar.Enabled = false;
            //btn_usuario_agregar.Enabled = true;
            //txt_usuario_cedula.Enabled = true;
            //btn_usuario_leer_tarjeta.Enabled = true;
            //if (picture_usuario_imagen.Image != null)
            //{
            //    picture_usuario_imagen.Image.Dispose();
            //    picture_usuario_imagen.Image = null;
            //}
        }
        private void AddUser_to_list(string action)
        {

            if (url_huella != "" || action == "Eliminar")
            {
                DataGridViewRow CurrentRow = DataGridView_Usuarios.CurrentRow;
                data_user_add.Rows.Add("",action, CurrentRow.Cells[1].Value.ToString(), CurrentRow.Cells[2].Value.ToString(),
                    CurrentRow.Cells[3].Value.ToString(), url_huella);
                
            }
            else
            {
                MessageBox.Show("No se registro la huella. Vuelva a intentarlo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            url_huella = "";
        }
        public void crear_usuario_sdk(string _numero_tarjeta, string _permisos_tarjeta, string _numero_usuario, string _nombre_usuario)
        {
            if (m_lSetCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle))
                {
                    m_lSetCardCfgHandle = -1;
                }
            }

            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lSetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Tarjeta.NET_DVR_SET_CARD, ptrStruCond, (int)struCond.dwSize, null, IntPtr.Zero);
            if (m_lSetCardCfgHandle < 0)
            {
                //Consultas.log("NET_DVR_SET_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); mostrar_log();
                //ObjConnectionPg.log("NET_DVR_SET_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); mostrar_log();
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                enviar_datos_tarjeta(_numero_tarjeta, _permisos_tarjeta, _numero_usuario, _nombre_usuario);
                Marshal.FreeHGlobal(ptrStruCond);
            }
        }

        private void enviar_datos_tarjeta(string _numero_tarjeta, string _permisos_tarjeta, string _numero_usuario, string _nombre_usuario)
        {
            HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD struData = new HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD();
            struData.Init();
            struData.dwSize = (uint)Marshal.SizeOf(struData);
            struData.byCardType = 1;
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struData.byCardNo[i] = byTempCardNo[i];
            }
            ushort.TryParse(_permisos_tarjeta, out struData.wCardRightPlan[0]);
            uint.TryParse(_numero_usuario, out struData.dwEmployeeNo);
            byte[] byTempName = new byte[HCNetSDK_Tarjeta.NAME_LEN];
            byTempName = System.Text.Encoding.Default.GetBytes(_nombre_usuario);
            for (int i = 0; i < byTempName.Length; i++)
            {
                struData.byName[i] = byTempName[i];
            }
            //periodo de validez del usuario
            struData.struValid.byEnable = 1;
            struData.struValid.struBeginTime.wYear = 2000;
            struData.struValid.struBeginTime.byMonth = 1;
            struData.struValid.struBeginTime.byDay = 1;
            struData.struValid.struBeginTime.byHour = 11;
            struData.struValid.struBeginTime.byMinute = 11;
            struData.struValid.struBeginTime.bySecond = 11;
            struData.struValid.struEndTime.wYear = 2030;
            struData.struValid.struEndTime.byMonth = 1;
            struData.struValid.struEndTime.byDay = 1;
            struData.struValid.struEndTime.byHour = 11;
            struData.struValid.struEndTime.byMinute = 11;
            struData.struValid.struEndTime.bySecond = 11;
            //permiso en puerta
            struData.byDoorRight[0] = 1;
            struData.wCardRightPlan[0] = 1;
            IntPtr ptrStruData = Marshal.AllocHGlobal((int)struData.dwSize);
            Marshal.StructureToPtr(struData, ptrStruData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
            uint dwReturned = 0;
            while (true)
            {
                dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lSetCardCfgHandle, ptrStruData, struData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                struStatus = (HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS)Marshal.PtrToStructure(ptrdwState, typeof(HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS));
                if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                {
                    Thread.Sleep(10);
                    continue;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                {
                    //Consultas.log("NET_DVR_SET_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); mostrar_log();
                    //ConnectionBDPostgres.log("NET_DVR_SET_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError(), Obj_ConnectionPg.conex); mostrar_log();
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                {
                    if (struStatus.dwErrorCode != 0)
                    {
                        //Consultas.log("NET_DVR_SET_CARD success but errorCode:" + struStatus.dwErrorCode); mostrar_log();
                        //ConnectionBDPostgres.log("NET_DVR_SET_CARD success but errorCode:" + struStatus.dwErrorCode, Obj_ConnectionPg.conex); mostrar_log();
                    }
                    else
                    {
                        //Consultas.log("NET_DVR_SET_CARD success"); mostrar_log();
                        //ConnectionBDPostgres.log("NET_DVR_SET_CARD success", Obj_ConnectionPg.conex); mostrar_log();
                    }
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                {
                    //Consultas.log("NET_DVR_SET_CARD finish"); mostrar_log();
                    //ConnectionBDPostgres.log("NET_DVR_SET_CARD finish", Obj_ConnectionPg.conex); mostrar_log();
                    break;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                {
                    //Consultas.log("NET_DVR_SET_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); mostrar_log();
                    //ConnectionBDPostgres.log("NET_DVR_SET_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError(), Obj_ConnectionPg.conex); mostrar_log();
                    break;
                }
                else
                {
                    //Consultas.log("unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); mostrar_log();
                    //.log("unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError(), Obj_ConnectionPg.conex); mostrar_log();
                    break;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle);
            m_lSetCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruData);
            Marshal.FreeHGlobal(ptrdwState);
            return;
        }

        private void enviar_rostro(string _numero_lectora, string _numero_tarjeta)
        {
            if (url_imagen == "")
            {
                MessageBox.Show("Por favor seleccione la ruta del rostro.","",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            //if (picture_usuario_imagen.Image != null)
            //{
            //    picture_usuario_imagen.Image.Dispose();
            //    picture_usuario_imagen.Image = null;
            //}

            CHCNetSDK_Facial.NET_DVR_FACE_COND struCond = new CHCNetSDK_Facial.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            struCond.dwFaceNum = 1;
            int.TryParse(_numero_lectora, out struCond.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            int dwInBufferSize = Convert.ToInt32(struCond.dwSize);
            IntPtr ptrstruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrstruCond, false);
            m_lSetFaceCfgHandle = CHCNetSDK_Facial.NET_DVR_StartRemoteConfig(Variables.m_UserID, CHCNetSDK_Facial.NET_DVR_SET_FACE, ptrstruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lSetFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrstruCond);
                //Consultas.log("NET_DVR_SET_FACE_FAIL, ERROR CODE" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString()); mostrar_log();
                //ConnectionBDPostgres.log("NET_DVR_SET_FACE_FAIL, ERROR CODE" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), Obj_ConnectionPg.conex); mostrar_log();
                return;
            }

            CHCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord = new CHCNetSDK_Facial.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);

            byte[] byRecordNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byRecordNo.Length; i++)
            {
                struRecord.byCardNo[i] = byRecordNo[i];
            }

            leer_datos_rostro(ref struRecord);
            int dwInBuffSize = Marshal.SizeOf(struRecord);
            int dwStatus = 0;

            CHCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus = new CHCNetSDK_Facial.NET_DVR_FACE_STATUS();
            struStatus.init();
            struStatus.dwSize = Marshal.SizeOf(struStatus);
            int dwOutBuffSize = Convert.ToInt32(struStatus.dwSize);
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));
            bool Flag = true;

            while (Flag)
            {
                dwStatus = CHCNetSDK_Facial.NET_DVR_SendWithRecvRemoteConfig(m_lSetFaceCfgHandle, ref struRecord, dwInBuffSize, ref struStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        proceso_enviar_datos_rostro(ref struStatus, ref Flag);
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        //Consultas.log("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString()); mostrar_log();
                        //.log("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), Obj_ConnectionPg.conex); mostrar_log();
                        Flag = false;
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        //Consultas.log("NET_SDK_GET_NEXT_STATUS_UNKOWN" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString()); mostrar_log();
                        //.log("NET_SDK_GET_NEXT_STATUS_UNKOWN" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), Obj_ConnectionPg.conex); mostrar_log();
                        Flag = false;
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrstruCond);
            Marshal.FreeHGlobal(ptrOutDataLen);
        }

        private void proceso_enviar_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus, ref bool flag)
        {
            switch (struStatus.byRecvStatus)
            {
                case 1:
                    //Consultas.log("SetFaceDataSuccessful"); mostrar_log();
                    //.log("SetFaceDataSuccessful", Obj_ConnectionPg.conex); mostrar_log();
                    break;
                default:
                    flag = false;
                    //Consultas.log("NET_SDK_SET_Face_DATA_FAILED" + struStatus.byRecvStatus.ToString()); mostrar_log();
                    //ConnectionBDPostgres.log("NET_SDK_SET_Face_DATA_FAILED" + struStatus.byRecvStatus.ToString(), Obj_ConnectionPg.conex); mostrar_log();
                    break;
            }

        }

        private void leer_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord)
        {
            //if (picture_usuario_imagen.Image != null)
            //{
            //    picture_usuario_imagen.Image.Dispose();
            //    picture_usuario_imagen.Image = null;
            //}

            if (!File.Exists(url_imagen))
            {
                MessageBox.Show("The face picture does not exist!");
                return;
            }
            FileStream fs = new FileStream(url_imagen, FileMode.OpenOrCreate);
            if (0 == fs.Length)
            {
                MessageBox.Show("The face picture is 0k,please input another picture!");
                return;
            }
            if (200 * 1024 < fs.Length)
            {
                MessageBox.Show("The face picture is larger than 200k,please input another picture!");
                return;
            }
            try
            {
                int.TryParse(fs.Length.ToString(), out struRecord.dwFaceLen);
                int iLen = Convert.ToInt32(struRecord.dwFaceLen);
                byte[] by = new byte[iLen];
                struRecord.pFaceBuffer = Marshal.AllocHGlobal(iLen);
                fs.Read(by, 0, iLen);
                Marshal.Copy(by, 0, struRecord.pFaceBuffer, iLen);
                fs.Close();
            }
            catch
            {
                MessageBox.Show("Read Face Data failed");
                fs.Close();
                return;
            }
        }

        public void eliminar_usuario_dispositivo(string _numero_lectora, string _numero_tarjeta)
        {
            if (m_lDelCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle))
                {
                    m_lDelCardCfgHandle = -1;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA struSendData = new HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA();
            struSendData.Init();
            struSendData.dwSize = (uint)Marshal.SizeOf(struSendData);
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struSendData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruSendData = Marshal.AllocHGlobal((int)struSendData.dwSize);
            Marshal.StructureToPtr(struSendData, ptrStruSendData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            m_lGetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Tarjeta.NET_DVR_DEL_CARD, ptrStruCond, (int)struCond.dwSize, null, this.Handle);
            if (m_lGetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_DEL_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
                uint dwReturned = 0;
                while (true)
                {
                    dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lGetCardCfgHandle, ptrStruSendData, struSendData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                    if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                    {
                        //MessageBox.Show("NET_DVR_DEL_CARD success");
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                    {
                        //MessageBox.Show("NET_DVR_DEL_CARD finish");
                        break;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                        break;
                    }
                    else
                    {
                        MessageBox.Show("unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                        break;
                    }
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle);
            m_lDelCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruSendData);
            Marshal.FreeHGlobal(ptrdwState);
        }

        private void control_de_puerta(int _salida, int _estado)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (HCNetSDK_ControlPuerta.NET_DVR_ControlGateway(Variables.m_UserID, _salida, Convert.ToUInt32(_estado)))
                {
                    MessageBox.Show("NET_DVR_ControlGateway: open door succeed");
                }
                else
                {
                    MessageBox.Show("NET_DVR_ControlGateway: open door failed error:" + HCNetSDK_ControlPuerta.NET_DVR_GetLastError());
                }
                id_tabla_dispositivos = "";
            }
            else
            {
                MessageBox.Show("Seleccione un dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Cursor.Current = Cursors.Default;
        }

        private void proceso_capturar_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg, ref bool flag)
        {
            if (0 == struFaceCfg.dwFacePicSize)
            {
                return;
            }

            DateTime dt = DateTime.Now;
            string nombre_ruta_archivo = Environment.CurrentDirectory + "\\imagenes\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";
            try
            {
                using (FileStream fs = new FileStream(nombre_ruta_archivo, FileMode.OpenOrCreate))
                {
                    int FaceLen = struFaceCfg.dwFacePicSize;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struFaceCfg.pFacePicBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }

                //picture_usuario_imagen.Image = Image.FromFile(nombre_ruta_archivo);
                url_imagen = nombre_ruta_archivo;
            }
            catch
            {
                flag = false;
                MessageBox.Show("Captura de datos incorrecto.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MsgCallback(int lCommand, ref HCNetSDK_Evento.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            switch (lCommand)
            {
                case HCNetSDK_Evento.COMM_ALARM_ACS:
                    obtener_numero_tajeta(pAlarmInfo);
                    break;
                default:
                    break;
            }
        }

        private void obtener_numero_tajeta(IntPtr pAlarmInfo)
        {
            HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO struAcsAlarmInfo = new HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO();
            struAcsAlarmInfo = (HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO));
            HCNetSDK_Evento.NET_DVR_LOG_V30 struFileInfo = new HCNetSDK_Evento.NET_DVR_LOG_V30();
            struFileInfo.dwMajorType = struAcsAlarmInfo.dwMajor;
            struFileInfo.dwMinorType = struAcsAlarmInfo.dwMinor;
            char[] csTmp = new char[256];
            String szInfo = new String(csTmp).TrimEnd('\0');
            String szInfoBuf = null;
            if (struAcsAlarmInfo.struAcsEventInfo.byCardNo[0] != 0)
            {
                szInfoBuf = System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.struAcsEventInfo.byCardNo).TrimEnd('\0');
                Variables.no_tarjeta = szInfoBuf;
            }
        }

        //private void agregar_usuario(string _nombre, string _no_usuario, string _nivel_acceso, string _no_tarjeta, string _imagen, bool _nuevo_registro)
        //{
        //    if (_nombre != "" && _no_usuario != "" && _nivel_acceso != "" && _no_tarjeta != "" && _imagen != "")
        //    {
        //        string palabra1 = "", palabra2 = "";
        //        if(_nuevo_registro == true)
        //        {
        //            palabra1 = "registró";
        //            palabra2 = "agregado";
        //        }
        //        else
        //        {
        //            palabra1 = "actualizó";
        //            palabra2 = "actualizado";
        //        }

        //        for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
        //        {
        //            string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
        //            int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
        //            string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
        //            string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

        //            Consultas.log("Inicia " + direccion_ip); mostrar_log();

        //            string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
        //            if (conexion == "OK")
        //            {
        //                crear_usuario_sdk(_no_tarjeta, _nivel_acceso, _no_usuario, _nombre);
        //                enviar_rostro("1", _no_tarjeta);
        //                Consultas.log("Termina " + direccion_ip); mostrar_log();
        //            }
        //            else
        //            {
        //                Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
        //            }
        //        }

        //        string consulta = "insert into usuario (nombre,no_usuario,no_tarjeta,permiso_tarjeta,imagen) values('" + _nombre + "'," + Convert.ToInt32(_no_usuario) + ",'" + _no_tarjeta + "'," + Convert.ToInt32(_nivel_acceso) + ",'" + _imagen + "')";
        //        Consultas.Insertar_Eliminar_Modificar(consulta);

        //        Consultas.log("Se "+ palabra1 + " el usuario " + _nombre); mostrar_log();
        //        limpiar_usuarios();
        //        mostrar_usuarios();
        //        MessageBox.Show("Usuario "+ palabra2 + " correctamente.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Complete todos los campos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //    Variables.m_UserID = -1;
        //}
        
        private void capturar_imagen()
        {
            if (m_lCapFaceCfgHandle != -1)
            {
                CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                m_lCapFaceCfgHandle = -1;
            }
            //if (picture_usuario_imagen.Image != null)
            //{
            //    picture_usuario_imagen.Image.Dispose();
            //    picture_usuario_imagen.Image = null;
            //}
            url_imagen = "";

            CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND struCond = new CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            m_lCapFaceCfgHandle = CHCNetSDK_Facial.NET_DVR_StartRemoteConfig(Variables.m_UserID, CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FACE_FAIL, ERROR CODE" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg = new CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG();
            struFaceCfg.init();
            int dwStatus = 0;
            int dwOutBuffSize = Marshal.SizeOf(struFaceCfg);
            bool Flag = true;
            while (Flag)
            {
                dwStatus = CHCNetSDK_Facial.NET_DVR_GetNextRemoteConfig(m_lCapFaceCfgHandle, ref struFaceCfg, dwOutBuffSize);
                switch (dwStatus)
                {
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        proceso_capturar_datos_rostro(ref struFaceCfg, ref Flag);
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);
        }

        private void enviar_huella(string _lectora, string _id_huella, string _no_tarjeta)
        {
            if (m_lSetFingerPrintCfgHandle != -1)
            {
                HCNetSDK_Huella.NET_DVR_StopRemoteConfig((int)m_lSetFingerPrintCfgHandle);
                m_lSetFingerPrintCfgHandle = -1;
            }

            HCNetSDK_Huella.NET_DVR_FINGERPRINT_COND strupond = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_COND();
            strupond.init();
            int dwSize = Marshal.SizeOf(strupond);
            strupond.dwSize = dwSize;
            byte.TryParse(_id_huella, out strupond.byFingerPrintID);
            int.TryParse(_lectora, out strupond.dwEnableReaderNo);
            strupond.dwFingerprintNum = 1;
            byte[] byTempptrRec = System.Text.Encoding.UTF8.GetBytes(_no_tarjeta);
            for (int i = 0; i < byTempptrRec.Length; i++)
            {
                strupond.byCardNo[i] = byTempptrRec[i];
            }

            IntPtr ptrStrucond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(strupond, ptrStrucond, false);

            m_lSetFingerPrintCfgHandle = HCNetSDK_Huella.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Huella.NET_DVR_SET_FINGERPRINT, ptrStrucond, dwSize, null, IntPtr.Zero);
            if (-1 == m_lSetFingerPrintCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStrucond);
                MessageBox.Show("NET_DVR_SET_FINGERPRINT_CFG_V50 FAIL, ERROR CODE" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            Boolean Flag = true;
            int dwStatus = 0;
            HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD StruRecord = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD();
            StruRecord.init();
            int dwInBuffSize = Marshal.SizeOf(StruRecord);
            StruRecord.dwSize = dwInBuffSize;
            byte.TryParse(_id_huella, out StruRecord.byFingerPrintID);
            int.TryParse(_lectora, out StruRecord.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(_no_tarjeta);
            for (int i = 0; i < byTemp.Length; i++)
            {
                StruRecord.byCardNo[i] = byTemp[i];
            }
            leer_datos_huella(ref StruRecord);

            HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS StruStatus = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS();
            StruStatus.init();
            int dwOutBuffSize = Marshal.SizeOf(StruStatus);
            StruStatus.dwSize = dwOutBuffSize;
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));


            while (Flag)
            {
                dwStatus = HCNetSDK_Huella.NET_DVR_SendWithRecvRemoteConfig(m_lSetFingerPrintCfgHandle, ref StruRecord, dwInBuffSize, ref StruStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        procesar_envio_datos_huella(ref StruStatus, ref Flag);
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        //Consultas.log("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString()); mostrar_log();
                        //ConnectionBDPostgres.log("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), Obj_ConnectionPg.conex); mostrar_log();
                        Flag = false;
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        Flag = false;
                        break;
                    default:
                        //Consultas.log("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString()); mostrar_log();
                        //ConnectionBDPostgres.log("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), Obj_ConnectionPg.conex); mostrar_log();
                        Flag = false;
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStrucond);
            Marshal.FreeHGlobal(ptrOutDataLen);
        }

        private void leer_datos_huella(ref HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD Record)
        {
            try
            {
                using (FileStream fs = new FileStream(url_huella, FileMode.OpenOrCreate))
                {
                    if (0 == fs.Length)
                    {
                        Record.byFingerData[0] = 0;
                        fs.Close();
                    }
                    Record.dwFingerPrintLen = (int)fs.Length;
                    BinaryReader objBinaryReader = new BinaryReader(fs);
                    if (Record.dwFingerPrintLen > HCNetSDK_Huella.MAX_FINGER_PRINT_LEN)
                    {
                        MessageBox.Show("FingerPrintLen is too long");
                        return;
                    }
                    for (int i = 0; i < Record.dwFingerPrintLen; i++)
                    {
                        if (i >= fs.Length)
                        {
                            break;
                        }
                        Record.byFingerData[i] = objBinaryReader.ReadByte();
                    }
                    fs.Close();
                }
            }
            catch
            {
                if (m_lSetFingerPrintCfgHandle != -1)
                {
                    HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                }
                MessageBox.Show("FingerDataPath may be wrong", "Error", MessageBoxButtons.OK);
            }
        }

        private void procesar_envio_datos_huella(ref HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS ststus, ref bool flag)
        {
            switch (ststus.byRecvStatus)
            {
                case 0:
                    //Consultas.log("SetFingegDataSuccessful"); mostrar_log();
                    //ConnectionBDPostgres.log("SetFingegDataSuccessful", Obj_ConnectionPg.conex); mostrar_log();
                    break;
                default:
                    flag = false;
                    ///Consultas.log("NET_SDK_SET_FINGER_DATA_FAILED" + ststus.byRecvStatus.ToString()); mostrar_log();
                    //ConnectionBDPostgres.log("NET_SDK_SET_FINGER_DATA_FAILED" + ststus.byRecvStatus.ToString(), Obj_ConnectionPg.conex); mostrar_log();
                    break;
            }
        }

        private void capturar_huella()
        {
            if (m_lCapFingerPrintCfHandle != -1)
            {
                HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                m_lCapFingerPrintCfHandle = -1;
            }

            HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_COND struCond = new HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            struCond.byFingerPrintPicType = 1;
            struCond.byFingerNo = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lCapFingerPrintCfHandle = HCNetSDK_Huella.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFingerPrintCfHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FINGERPRINT FAIL, ERROR CODE" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
            }

            bool flag = true;
            int dwStatus = 0;

            HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG struCFG = new HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG();
            struCFG.init();
            struCFG.dwSize = Marshal.SizeOf(struCFG);
            int dwOutBuffSize = struCFG.dwSize;
            while (flag)
            {
                dwStatus = HCNetSDK_Huella.NET_DVR_GetNextRemoteConfig(m_lCapFingerPrintCfHandle, ref struCFG, dwOutBuffSize);
                switch (dwStatus)
                {
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        procesar_datos_captura_huella(ref struCFG, ref flag);
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        flag = false;
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        flag = false;
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);
        }

        private void procesar_datos_captura_huella(ref HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG struCFG, ref bool flag)
        {
            DateTime dt = DateTime.Now;
            //string nombre_archivo = Environment.CurrentDirectory + "\\huellas2\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".dat";
            string nombre_archivo = Environment.CurrentDirectory + "\\huellas2\\" + DataGridView_Usuarios.CurrentRow.Cells["cedula_user"].Value.ToString() + ".dat";
            try
            {
                using (FileStream fs = new FileStream(nombre_archivo, FileMode.OpenOrCreate))
                {
                    fs.Write(struCFG.byFingerData, 0, struCFG.dwFingerPrintDataSize);
                    fs.Close();
                }
                url_huella = nombre_archivo;

                query = $"update empleados set huella='{url_huella}' where conse={id_tabla_usuarios}";
                ObjConnectionPg.SetConnetion();
                ObjConnectionPg.Update(query);
                ObjConnectionPg.CloseConnection();
                MessageBox.Show("Huella capturada con éxito","", MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
            catch
            {
                MessageBox.Show("CapFingerprint process failed");
                flag = false;
            }
        }

        //---------------------------------------------- EVENTOS

        private void btn_disp_eliminar_Click(object sender, EventArgs e)// ready
        {
            Cursor.Current = Cursors.WaitCursor;
            DialogResult dialogResult = MessageBox.Show("¿Realmente desea eliminar el dispositivo?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (id_tabla_dispositivos != "")
                {
                    //Consultas.Insertar_Eliminar_Modificar("delete from dispositivos where id=" + id_tabla_dispositivos);

                    query = $"delete from h_dispositivos where id = {id_tabla_dispositivos} ";
                    ObjConnectionPg.SetConnetion();
                    ObjConnectionPg.Delete(query);
                    ObjConnectionPg.CloseConnection();  

                    MessageBox.Show("Dispositivo eliminado correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar_dispositivos();
                    mostrar_dispositivos();
                }
                else
                {
                    MessageBox.Show("Seleccione un dispositivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                limpiar_dispositivos();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btn_disp_impiar_Click(object sender, EventArgs e)
        {
            limpiar_dispositivos();
        }

        private void btn_disp_agregar_Click(object sender, EventArgs e) //ready
        {
            Cursor.Current = Cursors.WaitCursor;
            if (txt_disp_direcc_ip.Text != "" && DropdownItems_pdv.Text != "" && txt_disp_puerto.Text != "" && txt_disp_usuario.Text != "" && 
                txt_disp_contrasena.Text != "")
            {
                IPAddress address;
                if (IPAddress.TryParse(txt_disp_direcc_ip.Text, out address) == true)
                {
                    
                    query = $"SELECT * FROM h_dispositivos where direccion_ip='{txt_disp_direcc_ip.Text}';";
                    //if (Consultas.Verificar_Dato("select count(id) as cant from dispositivos where direccion_ip='" + txt_disp_direcc_ip.Text + "'", "cant") == false)
                    if (ObjConnectionPg.Check_Data(query) == false)
                    {
                        string respuesta = Dispositivo.verificar_conexion(txt_disp_direcc_ip.Text, Convert.ToInt32(txt_disp_puerto.Text), txt_disp_usuario.Text, txt_disp_contrasena.Text);
                        if (respuesta == "OK")
                        {
                            string contrasena_encriptada = MD5Crypto.Encriptar(txt_disp_contrasena.Text);
                            bool estatus = false;
                            if (ping(txt_disp_direcc_ip.Text, 1000) == true)
                            {
                                estatus = true;
                            }
                            string[] PuntoVenta = DropdownItems_pdv.SelectedItem.ToString().Split('-');
                            
                            query = $"insert into h_dispositivos (descripcion,direccion_ip,pdv,puerto,usuario,contrasena,estatus) values ('{PuntoVenta[0].Trim()}','{txt_disp_direcc_ip.Text}',{PuntoVenta[1].Trim()},{Convert.ToInt32(txt_disp_puerto.Text)},'{txt_disp_usuario.Text}','{contrasena_encriptada}',{estatus});";
                            ObjConnectionPg.SetConnetion();
                            ObjConnectionPg.Insert(query);
                            ObjConnectionPg.CloseConnection();
                            
                            //string consulta = "insert into dispositivos (descripcion, direccion_ip,puerto, usuario, contrasena, estatus) values('" + txt_disp_descripcion.Text + "','" + txt_disp_direcc_ip.Text + "'," + Convert.ToInt32(txt_disp_puerto.Text) + ",'" + txt_disp_usuario.Text + "','" + contrasena_encriptada + "'," + estatus + ")";
                            //Consultas.Insertar_Eliminar_Modificar(consulta);
                            mostrar_dispositivos();
                            limpiar_dispositivos();
                            MessageBox.Show("Dispositivo agregado correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(respuesta, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El dispositivo ya se encuentra agregado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("El formato de la dirección IP es incorrecta.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Complete todos los campos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;
        }

        private void btn_usuario_buscar_imagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + "\\imagenes";
            openFileDialog.Filter = "Face file|*.jpg|All documents|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //if (picture_usuario_imagen.Image != null)
                //{
                //    picture_usuario_imagen.Image.Dispose();
                //    picture_usuario_imagen.Image = null;
                //}
                //url_imagen = openFileDialog.FileName;
                //picture_usuario_imagen.Image = Image.FromFile(url_imagen);
            }
            openFileDialog.Dispose();
        }

        private void btn_usuario_limpiar_Click(object sender, EventArgs e)
        {
            limpiar_usuarios();
        }

        private void btn_usuario_eliminar_Click(object sender, EventArgs e)// ready
        {
            Cursor.Current = Cursors.WaitCursor;
            if(data_user_add.Rows.Count != 0)
            {
                int d = data_user_add.CurrentRow.Index;
                data_user_add.Rows.Remove(data_user_add.Rows[d]);
            }
                    
            Cursor.Current = Cursors.Default;
        }

        private void btn_usuario_agregar_Click(object sender, EventArgs e)//ready 
        {
            Cursor.Current = Cursors.WaitCursor;
            
            if (id_tabla_usuarios != "") {
                if(url_huella != "")
                {
                    bool user_exist = false;
                    DataGridViewRow CurrentRow = DataGridView_Usuarios.CurrentRow; //DataGridViewRow();
                    foreach (DataGridViewRow row in data_user_add.Rows)
                    {
                        if (CurrentRow.Cells[1].Value.ToString() == row.Cells[1].Value.ToString())
                        {
                            user_exist = true;
                            break;
                        }
                    }
                    if (user_exist)
                    {
                        MessageBox.Show("El empleado ya esta agregado a la lista", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        data_user_add.Rows.Add("", CurrentRow.Cells[1].Value.ToString(), CurrentRow.Cells[2].Value.ToString(),
                            CurrentRow.Cells[3].Value.ToString(), CurrentRow.Cells[4].Value.ToString(), url_huella);
                    }
                }
                else
                {
                    MessageBox.Show("El empleado seleccionado aún no registra una huella", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
            }
            else
            {
                MessageBox.Show("Seleccione un empleado", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            Cursor.Current = Cursors.Default;
            url_huella = "";
        }

        private void data_dispositivos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id_tabla_dispositivos = data_dispositivos.CurrentRow.Cells["id"].Value.ToString();
                DropdownItems_pdv.Text = data_dispositivos.CurrentRow.Cells["descripcion"].Value.ToString();
                txt_disp_direcc_ip.Text = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                txt_disp_usuario.Text = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                txt_disp_contrasena.Text = data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString();
                txt_disp_puerto.Text = data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString();
                btn_disp_eliminar.Enabled = true;
            }
            catch { }
        }

        private void data_usuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id_tabla_usuarios = DataGridView_Usuarios.CurrentRow.Cells["id_usuario"].Value.ToString();
                //txt_usuario_nombre.Text = data_usuarios.CurrentRow.Cells["nombre"].Value.ToString();
                //txt_usuario_no_tarjeta.Text = data_usuarios.CurrentRow.Cells["no_tarjeta"].Value.ToString();
                //txt_usuario_cedula.Text = data_usuarios.CurrentRow.Cells["cedula_user"].Value.ToString();

                ////txt_usuario_niv_acceso.Text = data_usuarios.CurrentRow.Cells["permiso_tarjeta"].Value.ToString();
                ////btn_usuario_eliminar.Enabled = true;
                ////btn_usuario_agregar.Enabled = true;

                //txt_usuario_cedula.Enabled = false;
                //MessageBox.Show(id_tabla_usuarios);
            }
            catch { }
        }

        private void btn_disp_abrir_Click(object sender, EventArgs e)
        {
            control_de_puerta(1, 1); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            limpiar_dispositivos();
        }

        private void btn_disp_cerrar_Click(object sender, EventArgs e)
        {
            control_de_puerta(1, 0); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            limpiar_dispositivos();
        }

        private void btn_disp_mant_abierto_Click(object sender, EventArgs e)
        {
            control_de_puerta(1, 2); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            limpiar_dispositivos();
        }

        private void btn_disp_mant_cerrado_Click(object sender, EventArgs e)
        {
            control_de_puerta(1, 3); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            limpiar_dispositivos();
        }

        private void btn_usuario_tomar_foto_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    capturar_imagen();
                }
                else
                {
                    MessageBox.Show("El dispositivo " + direccion_ip + " está fuera de linea", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;
        }
        private void btn_SincOnedevice_Click(object sender, EventArgs e)
        {     
            Cursor.Current = Cursors.WaitCursor;
            bool Process = checkedListBox_Processes.GetItemChecked(0) | checkedListBox_Processes.GetItemChecked(1);
            if (Process)
            {
                if (id_tabla_dispositivos != "")
                {
                    string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                    int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                    string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                    string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (conexion == "OK")
                    {
                        //Cargar usuarios regostrados en base de datos//
                        if (checkedListBox_Processes.GetItemChecked(0))
                        {
                            int data_limit = 20;
                            int offset = 0;
                            ObjConnectionPg.SetConnetion();
                            
                            do
                            {
                                query = $"select conse,nombre,huella from empleados where huella != '' order by conse limit {data_limit} offset {offset}";
                                offset += data_limit;
                                tabla_usuarios.Rows.Clear();
                                tabla_usuarios = ObjConnectionPg.Select(query);

                                foreach (DataRow row in tabla_usuarios.Rows)
                                {
                                    string no_tarjeta = row["conse"].ToString();//row.Cells["no_tarjeta_add"].Value.ToString();
                                    string nombre = row["nombre"].ToString();
                                    string no_usuario = no_tarjeta;
                                    string permiso_tarjeta = "1";
                                    url_huella = row["huella"].ToString();
                                    crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                                    enviar_huella("1", "1", no_tarjeta);       
                                }
                            } 
                            while (tabla_usuarios.Rows.Count >= data_limit);     
                            ObjConnectionPg.CloseConnection();
                            MessageBox.Show("Los usuarios ya registrados se agregaron al dispositivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                        // Aplicar procesos de lista
                        if (checkedListBox_Processes.GetItemChecked(1))
                        {
                            if(data_user_add.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in data_user_add.Rows)
                                {
                                    string no_tarjeta = row.Cells["no_tarjeta_add"].Value.ToString(); //.Cells["no_tarjeta"].Value.ToString();
                                    if (row.Cells["process"].Value.ToString() == "Agregar")
                                    {
                                        string nombre = row.Cells["nombre_add"].Value.ToString();
                                        string no_usuario = no_tarjeta;
                                        string permiso_tarjeta = "1";
                                        url_huella = row.Cells["fingerprint"].Value.ToString();
                                        crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                                        enviar_huella("1", "1", no_tarjeta);
                                    }
                                    else
                                    {
                                        eliminar_usuario_dispositivo("1", no_tarjeta);
                                    }                      
                                }
                                MessageBox.Show("Procesos ejecutados correctamente sobre el dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                data_user_add.Rows.Clear();
                            }
                            else
                            {
                                MessageBox.Show("La lista no tiene procesos para aplicar.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dispisitivo " + direccion_ip + " fuera de linea.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    } 
                }
                else
                {
                    MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            
                //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
                Variables.m_UserID = -1;
            }
            else
            {
                MessageBox.Show("Seleccione una casilla", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Cursor.Current = Cursors.Default;
        }
        private void btn_SincAlldevice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool Process = checkedListBox_Processes.GetItemChecked(0) | checkedListBox_Processes.GetItemChecked(1);
            if (Process)
            {
                //Cargar usuarios regostrados en base de datos//
                if (checkedListBox_Processes.GetItemChecked(0))
                {
                    int data_limit = 20;
                    int offset = 0;
                    ObjConnectionPg.SetConnetion();
                    do
                    {
                        query = $"select conse,nombre,huella from empleados where huella != '' order by conse limit {data_limit} offset {offset}";
                        offset += data_limit;
                        tabla_usuarios.Rows.Clear();
                        tabla_usuarios = ObjConnectionPg.Select(query);
                        foreach (DataGridViewRow rowDisp in data_dispositivos.Rows)
                        {
                            string direccion_ip = rowDisp.Cells["direccion_ip"].Value.ToString();
                            int puerto = Convert.ToInt32(rowDisp.Cells["puerto"].Value.ToString());
                            string usuario = rowDisp.Cells["usuario"].Value.ToString();
                            string contrasena = MD5Crypto.Desencriptar(rowDisp.Cells["contrasena"].Value.ToString());

                            string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);

                            if (conexion == "OK")
                            {
                                foreach (DataRow row in tabla_usuarios.Rows)
                                {
                                    string no_tarjeta = row["conse"].ToString();//row.Cells["no_tarjeta_add"].Value.ToString();
                                    string nombre = row["nombre"].ToString();
                                    string no_usuario = no_tarjeta;
                                    string permiso_tarjeta = "1";
                                    url_huella = row["huella"].ToString();
                                    crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                                    enviar_huella("1", "1", no_tarjeta);
                                }                             
                            }
                            else
                            {
                                MessageBox.Show("Dispisitivo " + direccion_ip + " fuera de linea.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
                            Variables.m_UserID = -1;
                        }                     
                    }
                    while (tabla_usuarios.Rows.Count >= data_limit);
                    ObjConnectionPg.CloseConnection();
                    MessageBox.Show("Los usuarios ya registrados se agregaron en todos los dispositivos", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                // Aplicar procesos de lista
                if (checkedListBox_Processes.GetItemChecked(1))
                {
                    if (data_user_add.Rows.Count > 0 & data_dispositivos.Rows.Count > 0)
                    {
                        //for (int x = 0; x < data_dispositivos.Rows.Count; x++)
                        foreach(DataGridViewRow rowDisp in data_dispositivos.Rows)
                        {
                            string direccion_ip = rowDisp.Cells["direccion_ip"].Value.ToString();
                            int puerto = Convert.ToInt32(rowDisp.Cells["puerto"].Value.ToString());
                            string usuario = rowDisp.Cells["usuario"].Value.ToString();
                            string contrasena = MD5Crypto.Desencriptar(rowDisp.Cells["contrasena"].Value.ToString());

                            string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);

                            if (conexion == "OK")
                            {
                                foreach (DataGridViewRow rowUser in data_user_add.Rows)
                                {
                                    string no_tarjeta = rowUser.Cells["no_tarjeta_add"].Value.ToString(); //.Cells["no_tarjeta"].Value.ToString();

                                    if (rowUser.Cells["process"].Value.ToString() == "Agregar")
                                    {
                                        string nombre = rowUser.Cells["nombre_add"].Value.ToString();
                                        string no_usuario = no_tarjeta;
                                        string permiso_tarjeta = "1";
                                        url_huella = rowUser.Cells["fingerprint"].Value.ToString();
                                        crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                                        enviar_huella("1", "1", no_tarjeta);
                                    }
                                    else
                                    {
                                        eliminar_usuario_dispositivo("1", no_tarjeta);
                                    }
                                }
                                limpiar_usuarios();
                                //mostrar_usuarios();
                                data_user_add.Rows.Clear();
                                MessageBox.Show("Procesos ejecutados correctamente sobre los dispositivos.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {
                                MessageBox.Show("Dispisitivo " + direccion_ip + " fuera de linea.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
                            Variables.m_UserID = -1;

                        }

                    }
                    else
                    {
                        MessageBox.Show("Lista de procesos o Lista de dispositivos esta vacia.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una casilla", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Cursor.Current = Cursors.Default;
        }
        private void btn_usuario_leer_tarjeta_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    HCNetSDK_Evento.NET_DVR_SETUPALARM_PARAM struSetupAlarmParam = new HCNetSDK_Evento.NET_DVR_SETUPALARM_PARAM();
                    struSetupAlarmParam.dwSize = (uint)Marshal.SizeOf(struSetupAlarmParam);
                    struSetupAlarmParam.byLevel = 1;
                    struSetupAlarmParam.byAlarmInfoType = 1;
                    struSetupAlarmParam.byDeployType = (byte)1;

                    if (HCNetSDK_Evento.NET_DVR_SetupAlarmChan_V41(Variables.m_UserID, ref struSetupAlarmParam) < 0)
                    {
                        MessageBox.Show("NET_DVR_SetupAlarmChan_V41 fail error: " + HCNetSDK_Evento.NET_DVR_GetLastError(), "Setup alarm chan failed");
                        return;
                    }

                    m_falarmData = new HCNetSDK_Evento.MSGCallBack(MsgCallback);
                    if (!HCNetSDK_Evento.NET_DVR_SetDVRMessageCallBack_V50(0, m_falarmData, IntPtr.Zero))
                    {
                        MessageBox.Show("NET_DVR_SetDVRMessageCallBack_V50 fail", "operation fail", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    //Consultas.log("Dispositivo " + direccion_ip + " fuera de linea."); mostrar_log();
                    //ConnectionBDPostgres.log("Dispositivo " + direccion_ip + " fuera de linea.", Obj_ConnectionPg.conex); mostrar_log();
                }
                timer_no_tarjeta.Enabled = true;
                groupBox_usuarios.Enabled = false;
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;
        }

        private void timer_no_tarjeta_Tick(object sender, EventArgs e)
        {
            //txt_usuario_no_tarjeta.Text = Variables.no_tarjeta;
            if(Variables.no_tarjeta != "Pase la tarjeta...")
            {
                timer_no_tarjeta.Enabled = false;
                groupBox_usuarios.Enabled = true;
                Variables.no_tarjeta = "Pase la tarjeta...";
                limpiar_dispositivos();
            }
            tiempo_timer_tarjeta++;
            if (tiempo_timer_tarjeta > 20)
            {
                timer_no_tarjeta.Enabled = false;
                groupBox_usuarios.Enabled = true;
                Variables.no_tarjeta = "Pase la tarjeta...";
                //txt_usuario_no_tarjeta.Text = "";
                tiempo_timer_tarjeta = 0;
                limpiar_dispositivos();
                MessageBox.Show("Tiempo de espera agotado","",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void btn_usuario_actualizar_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //if (id_tabla_usuarios != "")
            //{
            //    for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
            //    {
            //        string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
            //        int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
            //        string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
            //        string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

            //        string no_tarjeta_anterior = Consultas.Seleccionar_String("select no_tarjeta from usuario where id=" + id_tabla_usuarios, "no_tarjeta");

            //        string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
            //        if (conexion == "OK")
            //        {
            //            //eliminar_usuario_dispositivo("1", no_tarjeta_anterior);
            //            crear_usuario_sdk(txt_usuario_no_tarjeta.Text, txt_usuario_niv_acceso.Text, txt_usuario_no_usuario.Text, txt_usuario_nombre.Text);
            //            enviar_rostro("1", txt_usuario_no_tarjeta.Text);

            //            eliminar_usuario_dispositivo("1", no_tarjeta_anterior);
            //        }
            //        else
            //        {
            //            Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
            //        }
            //    }
            //    string consulta = "update usuario set nombre='" + txt_usuario_nombre.Text + "', no_tarjeta='" + txt_usuario_no_tarjeta.Text + "', permiso_tarjeta=" + Convert.ToInt32(txt_usuario_niv_acceso.Text) + ", imagen='" + url_imagen + "' where id=" + id_tabla_usuarios;
            //    Consultas.Insertar_Eliminar_Modificar(consulta);
            //    limpiar_usuarios();
            //    mostrar_usuarios();
            //}
            //else
            //{
            //    MessageBox.Show("Seleccione un usuario", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //Variables.m_UserID = -1;
            //Cursor.Current = Cursors.Default;
        }        
        private void btn_usu_sinc_todos_a_todos_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_usuarios != "")
            {
                for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
                {
                    string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
                    int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
                    string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
                    string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

                    //Consultas.log("Inicia " + direccion_ip); mostrar_log();
                    //ConnectionBDPostgres.log("Inicia " + direccion_ip, Obj_ConnectionPg.conex); mostrar_log();

                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (conexion == "OK")
                    {
                        for (int y = 0; y < tabla_usuarios.Rows.Count; y++)
                        {
                            string no_tarjeta = DataGridView_Usuarios.Rows[y].Cells["no_tarjeta"].Value.ToString();
                            string permiso_tarjeta = DataGridView_Usuarios.Rows[y].Cells["permiso_tarjeta"].Value.ToString();
                            string no_usuario = DataGridView_Usuarios.Rows[y].Cells["no_usuario"].Value.ToString();
                            string nombre = DataGridView_Usuarios.Rows[y].Cells["nombre"].Value.ToString();
                            url_imagen = DataGridView_Usuarios.Rows[y].Cells["imagen"].Value.ToString();

                            crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                            enviar_rostro("1", no_tarjeta);
                        }
                        //Consultas.log("Termina " + direccion_ip); mostrar_log();
                        //ConnectionBDPostgres.log("Termina " + direccion_ip, Obj_ConnectionPg.conex); mostrar_log();
                    }
                    else
                    {
                        //Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                        //ConnectionBDPostgres.log("Dispisitivo " + direccion_ip + " fuera de linea.", Obj_ConnectionPg.conex); mostrar_log();
                    }
                }

                //Consultas.log("Se registró el usuario " + nombre); mostrar_log();
                //ConnectionBDPostgres.log("Se registró el usuario " + nombre, Obj_ConnectionPg.conex); mostrar_log();
                limpiar_usuarios();
                mostrar_usuarios();
                MessageBox.Show("Usuario sincronizado correctamente.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Complete todos los campos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;
        }

        private void btn_usuario_buscar_huella_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + "\\huellas";
            openFileDialog.Filter = "Fingerprint file|*.dat|All documents|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                url_huella = openFileDialog.FileName;
            }
        }

        private void btn_eventos_mostrar_Click(object sender, EventArgs e)
        {
            if (id_tabla_dispositivos != "")
            {
                Variables.evento_fecha_inicio = txt_evento_fecha_inicio.Text;
                Variables.evento_fecha_fin = txt_evento_fecha_fin.Text;
                Variables.evento_tipo_principal = txt_evento_tipo_principal.SelectedItem.ToString();
                Variables.evento_tipo_secundario = txt_evento_tipo_secundario.SelectedItem.ToString();
                Variables.direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString(); ;
                Variables.puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString()); ;
                Variables.usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                Variables.contrasena = data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString();

                eventos abrir = new eventos();
                //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
                abrir.Show();
            }
            else
            {
                MessageBox.Show("Seleccione un dispositivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void txt_usuario_nombre_DropDown(object sender, EventArgs e)
        {
            
            if (DropdownItems_pdv.Text != "")
            {
                MessageBox.Show("hola");
            }
            else
            {
                MessageBox.Show("seleccione un dispositivo");
            }

        }

        private void btn_find_user_Click(object sender, EventArgs e)
        {
            if(txt_search_user.Text != "")
            { 
                mostrar_usuarios();
            }
            else
            {
                MessageBox.Show("Complete el campo de busqueda.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DropdownItems_pdv_DropDown(object sender, EventArgs e)
        {
            DropdownItems_pdv.Items.AddRange(Variables.DropdownItems_pdv.ToArray());
        }

        
        private void button_AddUser_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                if (id_tabla_usuarios != "")
                {
                    if (!Check_user_in_list())
                    {
                        url_huella = DataGridView_Usuarios.CurrentRow.Cells["huella"].Value.ToString();
                        if ( url_huella == "")
                        {
                            RegisterFingerprint();
                        }
                        else
                        {
                            DialogResult dialogResult;
                            dialogResult = MessageBox.Show("El usuario ya registra una huella, desea actuallizarla ?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if(dialogResult == DialogResult.OK)
                            {
                                RegisterFingerprint();
                            }
                            else
                            {                           
                                AddUser_to_list("Agregar");
                            }
                        }


                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un empleado", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo para tomar la huella", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //MessageBox.Show("m_UserID:" + Variables.m_UserID + "\nResultado de Logout:" + HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            HCNetSDK_Tarjeta.NET_DVR_Logout(Variables.m_UserID);
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;
        }
        private void RegisterFingerprint()
        {
            string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
            int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
            string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
            string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());
            //MessageBox.Show($"ip:{direccion_ip.Count()},puerto:{puerto}, usu:{usuario},contra:{contrasena}");
            string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
            if (conexion == "OK")
            {
                DialogResult result;
                result = MessageBox.Show("Listo para tomar la huella ?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    capturar_huella();
                    AddUser_to_list("Agregar");
                }
            }
            else
            {
                MessageBox.Show("El dispositivo " + direccion_ip + " está fuera de linea", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        } 

        private void button_DeleteUser_Click(object sender, EventArgs e)
        {
            if (!Check_user_in_list())
            {
                AddUser_to_list("Eliminar");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string message = $"Todos los empleados que registren huella en la base de datos," +
                $" se ingresaran al dispositivo";
            toolTip_MessageNewDevice.SetToolTip(btn_SincNewdevice, message);
        }
    }
}
