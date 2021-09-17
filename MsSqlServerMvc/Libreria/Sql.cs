using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using Newtonsoft.Json;

namespace MsSqlServerMvc.Libreria
{
    public class Sql
    {
        public string Servidor { set; get; } = string.Empty;
        public string Usuario { set; get; } = string.Empty;
        public string Contrasenia { set; get; } = string.Empty;
        public string BaseDatos { set; get; } = string.Empty;

        public void Guardar(Sql mariaDb)
        {
            try
            {
                if (!Directory.Exists(@"C:\LJChuelloMvc"))
                {
                    Directory.CreateDirectory(@"C:\LJChuelloMvc");
                }
                File.WriteAllText(@"C:\LJChuelloMvc\LJChuelloMvc", JsonConvert.SerializeObject(mariaDb));
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        public Sql Leer()
        {
            Sql mariaDb = new Sql();
            try
            {
                return JsonConvert.DeserializeObject<Sql>(File.ReadAllText(@"C:\LJChuelloMvc\LJChuelloMvc"));
            }
            catch (Exception)
            {
                return mariaDb;
            }
        }

        public bool Validar(Page page, Sql sqlServer)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection())
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                    sqlConnectionStringBuilder.DataSource = sqlServer.Servidor;
                    sqlConnectionStringBuilder.UserID = sqlServer.Usuario;
                    sqlConnectionStringBuilder.Password = sqlServer.Contrasenia;
                    sqlConnectionStringBuilder.InitialCatalog = sqlServer.BaseDatos;
                    sqlConnectionStringBuilder.ConnectTimeout = 5;
                    sqlConnection.ConnectionString = sqlConnectionStringBuilder.ToString();
                    sqlConnection.Open();
                    SqlCommand mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = sqlConnection;
                    mySqlCommand.CommandType = CommandType.Text;
                    mySqlCommand.CommandText = "SELECT @@VERSION";
                    mySqlCommand.ExecuteNonQuery();
                }

                // Guardamos la conexion
                Guardar(sqlServer);

                // Libre de pecados
                return true;
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
                return false;
            }
        }

        public List<string> Tables_List(Page page, Sql sqlServer)
        {
            List<string> list = new List<string>();
            try
            {
                using (SqlConnection mySqlConnection = new SqlConnection())
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                    sqlConnectionStringBuilder.DataSource = sqlServer.Servidor;
                    sqlConnectionStringBuilder.UserID = sqlServer.Usuario;
                    sqlConnectionStringBuilder.Password = sqlServer.Contrasenia;
                    sqlConnectionStringBuilder.InitialCatalog = sqlServer.BaseDatos;
                    sqlConnectionStringBuilder.ConnectTimeout = 5;
                    mySqlConnection.ConnectionString = sqlConnectionStringBuilder.ToString();
                    mySqlConnection.Open();
                    SqlCommand mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandType = CommandType.Text;
                    mySqlCommand.CommandText = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
                    using (SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            list.Add($"{mySqlDataReader[0]}");
                        }
                    }
                }

                // Libre de pecados
                return list;
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
                return list;
            }
        }

        public List<Campos> Table_Details(Page page, Sql sqlServer, string tabla)
        {
            List<Campos> list = new List<Campos>();
            try
            {
                using (SqlConnection mySqlConnection = new SqlConnection())
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                    sqlConnectionStringBuilder.DataSource = sqlServer.Servidor;
                    sqlConnectionStringBuilder.UserID = sqlServer.Usuario;
                    sqlConnectionStringBuilder.Password = sqlServer.Contrasenia;
                    sqlConnectionStringBuilder.InitialCatalog = sqlServer.BaseDatos;
                    sqlConnectionStringBuilder.ConnectTimeout = 5;
                    mySqlConnection.ConnectionString = sqlConnectionStringBuilder.ToString();
                    mySqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = mySqlConnection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = $"SELECT" +
                                               "\nc.name AS 'Campo'," +
                                               "\nt.name AS 'Tipo'," +
                                               "\n--c.max_length AS 'Largo'," +
                                               "\n--c.precision AS 'Precision'," +
                                               "\nISNULL(is_primary_key, 0) AS 'PK'" +
                                               "\nFROM sys.columns c" +
                                               "\nINNER JOIN  sys.types t ON" +
                                               "\nc.user_type_id = t.user_type_id" +
                                               "\nLEFT OUTER JOIN sys.index_columns ic ON" +
                                               "\nic.object_id = c.object_id AND" +
                                               "\nic.column_id = c.column_id" +
                                               "\nLEFT OUTER JOIN sys.indexes i ON" +
                                               "\nic.object_id = i.object_id AND" +
                                               "\nic.index_id = i.index_id" +
                                               $"\nWHERE c.object_id = OBJECT_ID('{tabla}'); ";
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Campos campos = new Campos();
                            campos.Nombre = $"{sqlDataReader["Campo"]}";
                            campos.TipoSql = $"{sqlDataReader["Tipo"]}";
                            campos.Pk = Convert.ToBoolean(sqlDataReader["PK"]);

                            switch (campos.TipoSql)
                            {
                                case "nvarchar":
                                    campos.TipoDotNet = "string";
                                    break;

                                case "int":
                                    campos.TipoDotNet = "int";
                                    break;

                                case "datetime":
                                    campos.TipoDotNet = "DateTime";
                                    break;

                                case "decimal":
                                    campos.TipoDotNet = "decimal";
                                    break;

                                case "bit":
                                    campos.TipoDotNet = "bool";
                                    break;

                                default:
                                    campos.TipoDotNet = "xXxXx";
                                    break;
                            }

                            list.Add(campos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
            }

            return list;
        }

        public class Campos
        {
            public string Nombre { set; get; } = string.Empty;
            public string TipoSql { set; get; } = string.Empty;
            public string TipoDotNet { set; get; } = string.Empty;
            public bool Pk { set; get; } = false;
        }
    }
}