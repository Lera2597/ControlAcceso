using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo_sdk_hikvision
{
    internal class ConnectionPostgres
    {

        NpgsqlConnection _connection = new NpgsqlConnection();
        Dictionary<string, string> Info_Database = new Dictionary<string, string>();

        public ConnectionPostgres()
        {
            try
            {
                StreamReader sr = new StreamReader("InfoDataBase.txt");
                var line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    string[] data = line.Split('=');
                    Info_Database[data[0]] = data[1];
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Error en lectura de archivo:\n" + ex1.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public void SetConnetion()
        {
            String cadenaConexion = $"server={Info_Database["Ip_server"]};port={Info_Database["Port"]}; user id={Info_Database["User"]};password={Info_Database["Password"]};database={Info_Database["Database"]};";
            _connection.ConnectionString = cadenaConexion;
            try
            {
                _connection.Open();
                string q = $"set client_encoding to LATIN1";//Para que la BD acepte caracteres espciales 
                NpgsqlCommand conector = new NpgsqlCommand(q, _connection);
                conector.ExecuteNonQuery();
                //MessageBox.Show("Se conectó correctamente a la Base de datos");

            }

            catch (NpgsqlException ex2)
            {
                MessageBox.Show("Error en la conexion a base de datos:\n" + ex2.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public void CloseConnection()
        {
            _connection.Close();
        }
        public DataTable Select(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand comand = new NpgsqlCommand(query, _connection);
                NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter(comand);
                adaptador.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;


        }

        public bool Check_Data(string query)
        {
            NpgsqlCommand comand = new NpgsqlCommand(query, _connection);
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comand);
            DataTable dt = new DataTable();
            if (adapter.Fill(dt) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void Insert(string query)
        {
            Insert_Delete_Modify(query);
        }
        public void Delete(string query)
        {
            Insert_Delete_Modify(query);
        }
        public void Update(string query)
        {
            Insert_Delete_Modify(query);
        }
        private void Insert_Delete_Modify(string query)
        {
            try
            {
                NpgsqlCommand comand = new NpgsqlCommand(query, _connection);
                comand.ExecuteNonQuery();
                //MessageBox.Show("Se conectó correctamente a la Base de datos");
            }

            catch (NpgsqlException e)
            {
                MessageBox.Show(e.ToString());

            }
        }
        public void log(string description)
        {
            string fecha = DateTime.Now.ToString("g");
            string query = $"insert into h_log(fecha,descripcion)values('{fecha}','{description}')";
            Insert_Delete_Modify(query);
        }

    }
}
