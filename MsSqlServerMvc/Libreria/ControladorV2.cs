﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class ControladorV2
    {
        private int iteracion = 0;

        public string Generar(Page page, List<Sql.Campos> list, string tabla)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                // Cabecera
                stringBuilder.AppendLine($"// Co{tabla}.cs");
                stringBuilder.AppendLine($"// Clase generada por");
                stringBuilder.AppendLine($"// Leonardo Chuello");
                stringBuilder.AppendLine($"// {DateTime.Now:yyyy-MM-dd}");
                stringBuilder.AppendLine($"");
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using System.Data;");
                stringBuilder.AppendLine("using System.Text;");
                stringBuilder.AppendLine("using System.Threading.Tasks;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace Controlador");
                stringBuilder.AppendLine("{");

                stringBuilder.AppendLine($"    public class Co{tabla}");
                stringBuilder.AppendLine("    {");
                //stringBuilder.AppendLine($"        private readonly MsSql PoolConexion = new MsSql();");
                stringBuilder.AppendLine($"        private readonly string _select = \"SELECT {string.Join(", ", list.Select(x => x.Nombre).ToList())}\";");
                stringBuilder.AppendLine($"");

                #region Id

                Sql.Campos id = list.FirstOrDefault() ?? new Sql.Campos();

                if (!Cadena.Vacia(id.Nombre))
                {
                    stringBuilder.AppendLine($"        public async Task<Mo{tabla}> Select_{id.Nombre}_Async({id.TipoDotNet} {id.Nombre})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            return await Task.Run(() =>");
                    stringBuilder.AppendLine($"            {{");
                    stringBuilder.AppendLine($"                Mo{tabla} mo{tabla} = new Mo{tabla}();");
                    stringBuilder.AppendLine($"                try");
                    stringBuilder.AppendLine($"                {{");
                    stringBuilder.AppendLine($"                    SqlCommand sqlCommand = new SqlCommand();");
                    stringBuilder.AppendLine($"                    sqlCommand.Connection = Conexion.Devolver();");
                    stringBuilder.AppendLine($"                    sqlCommand.CommandType = CommandType.Text;");
                    stringBuilder.AppendLine($"                    sqlCommand.CommandText = $\"{{_select}} FROM {tabla} WHERE {id.Nombre} = @{id.Nombre};\";");
                    stringBuilder.AppendLine($"                    sqlCommand.Parameters.AddWithValue(\"@{id.Nombre}\", {id.Nombre});");
                    stringBuilder.AppendLine($"                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())");
                    stringBuilder.AppendLine($"                    {{");
                    stringBuilder.AppendLine($"                        while (sqlDataReader.Read())");
                    stringBuilder.AppendLine($"                        {{");
                    stringBuilder.AppendLine($"                            mo{tabla} = Maker(sqlDataReader);");
                    stringBuilder.AppendLine($"                        }}");
                    stringBuilder.AppendLine($"                    }}");
                    stringBuilder.AppendLine($"                    return mo{tabla};");
                    stringBuilder.AppendLine($"                }}");
                    stringBuilder.AppendLine($"                catch (Exception ex)");
                    stringBuilder.AppendLine($"                {{");
                    stringBuilder.AppendLine($"                    Console.WriteLine(ex);");
                    stringBuilder.AppendLine($"                    return mo{tabla};");
                    stringBuilder.AppendLine($"                }}");
                    stringBuilder.AppendLine($"            }});");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                #region Maker

                stringBuilder.AppendLine($"");
                stringBuilder.AppendLine($"        private Mo{tabla} Maker(SqlDataReader dtReader)");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            try");
                stringBuilder.AppendLine($"            {{");
                stringBuilder.AppendLine($"                Mo{tabla} mo{tabla} = new Mo{tabla}();");

                foreach (var row in list)
                {
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetInt32(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetDecimal(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = !dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) && dtReader.GetBoolean(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? new DateTime(1900, 01, 01) : dtReader.GetDateTime(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        default:
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? string.Empty : dtReader.GetString(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;
                    }
                }

                stringBuilder.AppendLine($"                return mo{tabla};");
                stringBuilder.AppendLine($"            }}");
                stringBuilder.AppendLine($"            catch (Exception ex)");
                stringBuilder.AppendLine($"            {{");
                stringBuilder.AppendLine($"                Console.WriteLine(ex);");
                stringBuilder.AppendLine($"                return new Mo{tabla}();");
                stringBuilder.AppendLine($"            }}");
                stringBuilder.AppendLine($"        }}");

                #endregion

                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
    }
}