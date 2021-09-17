using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class Controlador
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

                if (list.Count(x => x.Nombre == "Id") > 0)
                {
                    stringBuilder.AppendLine($"        public async Task<Mo{tabla}> Select_Id_Async(string id)");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            return await Task.Run(() =>");
                    stringBuilder.AppendLine($"            {{");
                    stringBuilder.AppendLine($"                Mo{tabla} mo{tabla} = new Mo{tabla}();");
                    stringBuilder.AppendLine($"                try");
                    stringBuilder.AppendLine($"                {{");
                    stringBuilder.AppendLine($"                    SqlCommand sqlCommand = new SqlCommand();");
                    stringBuilder.AppendLine($"                    sqlCommand.Connection = PoolConexion.Devolver();");
                    stringBuilder.AppendLine($"                    sqlCommand.CommandType = CommandType.Text;");
                    stringBuilder.AppendLine($"                    sqlCommand.CommandText = $\"{{_select}} FROM {tabla} WHERE Id = @Id;\";");
                    stringBuilder.AppendLine($"                    sqlCommand.Parameters.AddWithValue(\"@Id\", id);");
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

                #region Insert Block

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"        public string Insert_Block(Mo{tabla} mo{tabla})");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Insert {tabla}\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"INSERT INTO {tabla} (\");");

                // Cabecera
                iteracion = 0;
                foreach (var row in list)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != list.Count
                        ? $"            stringBuilder.AppendLine(\"{row.Nombre}, -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine(\"{row.Nombre} -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                // Values
                stringBuilder.AppendLine($"stringBuilder.AppendLine(\") VALUES (\");");

                //Detalles
                iteracion = 0;
                foreach (var row in list)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != list.Count
                        ? $"            stringBuilder.AppendLine($\"{{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}}, -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"{{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}}); -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                stringBuilder.AppendLine($"        }}");

                #endregion

                #region Update Block

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"        public string Update_Block(Mo{tabla} mo{tabla})");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Update {tabla}\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"UPDATE {tabla} SET\");");

                // Cabecera
                iteracion = 0;
                var listUpdate01 = list.Where(x => x.Nombre.ToLower() != "id").ToList();
                foreach (var row in listUpdate01)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != listUpdate01.Count
                        ? $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}}, -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}} -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                // Where
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"WHERE\");");
                iteracion = 0;
                List<Sql.Campos> listUpdate02 = new List<Sql.Campos>();
                listUpdate02.Add(list.FirstOrDefault(x => x.Nombre.ToLower() == "id"));
                foreach (var row in listUpdate02.Where(x => x != null))
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != listUpdate02.Count
                        ? $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}} AND -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}}; -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                stringBuilder.AppendLine($"        }}");

                #endregion

                #region Delete Block

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"        public string Delete_Block(Mo{tabla} mo{tabla})");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Delete {tabla}\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"DELETE FROM {tabla} WHERE\");");

                // Cabecera
                iteracion = 0;
                foreach (var row in list.Where(x => x.Pk).ToList())
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != list.Count(x => x.Pk)
                        ? $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}} AND -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"{row.Nombre} = {{PoolConexion.Remplazar(mo{tabla}.{row.Nombre})}}; -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                stringBuilder.AppendLine($"        }}");

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