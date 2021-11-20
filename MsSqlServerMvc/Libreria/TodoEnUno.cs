using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class TodoEnUno
    {
        public string Generar(Page page, List<Sql.Campos> campos, string tabla)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // iteracion
            int iteracion = 0;

            stringBuilder.AppendLine($"// {tabla}.cs");
            stringBuilder.AppendLine($"// Clase generada por");
            stringBuilder.AppendLine($"// Leonardo Chuello");
            stringBuilder.AppendLine($"// {DateTime.Now:yyyy-MM-dd}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"using System;");
            stringBuilder.AppendLine($"using System.Data.SqlClient;");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"// ReSharper disable once CheckNamespace");
            stringBuilder.AppendLine($"namespace DataCloud");
            stringBuilder.AppendLine($"{{");
            stringBuilder.AppendLine($"    // ReSharper disable once InconsistentNaming");
            stringBuilder.AppendLine($"    public class {tabla}");
            stringBuilder.AppendLine($"    {{");

            #region Métodos

            stringBuilder.AppendLine($"        private const string _select = \"SELECT {string.Join(", ", campos.Select(x => x.Nombre))} FROM {tabla}\";");
            stringBuilder.AppendLine("");

            #endregion

            #region Campos

            stringBuilder.AppendLine($"       #region Campos");
            stringBuilder.AppendLine($"");

            foreach (var campo in campos)
            {
                switch (campo.TipoDotNet)
                {
                    case "string":
                        stringBuilder.AppendLine($"        public string {campo.Nombre} {{ set; get; }} = string.Empty;");
                        break;

                    case "int":
                        stringBuilder.AppendLine($"        public int {campo.Nombre} {{ set; get; }} = 0;");
                        break;

                    case "decimal":
                        stringBuilder.AppendLine($"        public decimal {campo.Nombre} {{ set; get; }} = 0;");
                        break;

                    case "DateTime":
                        stringBuilder.AppendLine($"        public DateTime {campo.Nombre} {{ set; get; }} = new DateTime(1900, 01, 01);");
                        break;

                    case "bool":
                        stringBuilder.AppendLine($"        public bool {campo.Nombre} {{ set; get; }} = false;");
                        break;

                    default:
                        stringBuilder.AppendLine($"        public string {campo.Nombre} {{ set; get; }} = string.Empty;");
                        break;
                }
            }

            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"       #endregion");
            #endregion

            #region Métodos

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("       #region Methods");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("       #endregion");

            #endregion

            #region Bloque

            if (campos.First().Nombre.ToLower() == "id")
            {
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        #region Block's");

                #region Insert Block

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"        public string Insert_Block({tabla} {Cadena.PriMin(tabla)})");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Insert {tabla}\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"INSERT INTO {tabla} (\");");

                // Cabecera
                iteracion = 0;
                foreach (var row in campos)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != campos.Count
                        ? $"            stringBuilder.AppendLine(\"{row.Nombre}, -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine(\"{row.Nombre} -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                // Values
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\") VALUES (\");");

                //Detalles
                iteracion = 0;
                foreach (var row in campos)
                {
                    ++iteracion;

                    stringBuilder.AppendLine(iteracion != campos.Count
                        ? $"            stringBuilder.AppendLine($\"'{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"'{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}'); -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                }

                stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                stringBuilder.AppendLine($"        }}");

                #endregion

                #region Update Block

                if (campos.Count(x => x.Nombre.ToLower() == "id") == 1)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"        public string Update_Block({tabla} {Cadena.PriMin(tabla)})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Update {tabla}\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"UPDATE {tabla} SET\");");

                    // Cabecera
                    iteracion = 0;
                    var listUpdate01 = campos.Where(x => x.Nombre.ToLower() != "id").ToList();
                    foreach (var row in listUpdate01)
                    {
                        iteracion = iteracion + 1;
                        stringBuilder.AppendLine(iteracion != listUpdate01.Count
                            ? $"            stringBuilder.AppendLine($\"{row.Nombre} = '{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");"
                            : $"            stringBuilder.AppendLine($\"{row.Nombre} = '{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}' -- {row.Nombre} | {row.TipoSql} | {row.TipoDotNet}\");");
                    }

                    // Where
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"WHERE\");");
                    var rowUpdate = campos.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Sql.Campos();
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"{rowUpdate.Nombre} = '{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{rowUpdate.Nombre})}}'; -- {rowUpdate.Nombre} | {rowUpdate.TipoSql} | {rowUpdate.TipoDotNet}\");");
                    stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                #region Delete Block

                if (campos.Count(x => x.Nombre.ToLower() == "id") == 1)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"        public string Delete_Block({tabla} {Cadena.PriMin(tabla)})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Delete {tabla}\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"DELETE FROM {tabla} WHERE\");");
                    var rowDelete = campos.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Sql.Campos();
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"{rowDelete.Nombre} = '{{PoolConexion.Remplazar({Cadena.PriMin(tabla)}.{rowDelete.Nombre})}}'; -- {rowDelete.Nombre} | {rowDelete.TipoSql} | {rowDelete.TipoDotNet}\");");
                    stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                stringBuilder.AppendLine("        #endregion");
            }

            #endregion

            #region Maker

            stringBuilder.AppendLine("\n        #region Maker");

            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"        private {tabla} Maker(SqlDataReader dtReader)");
            stringBuilder.AppendLine($"        {{");
            stringBuilder.AppendLine($"            " +
                                     $"{tabla} {Cadena.PriMin(tabla)} = new {tabla}();");

            foreach (var row in campos)
            {
                switch (row.TipoDotNet)
                {
                    case "int":
                        stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetInt32(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                        break;

                    case "decimal":
                        stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetDecimal(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                        break;

                    case "bool":
                        stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = !dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) && dtReader.GetBoolean(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                        break;

                    case "DateTime":
                        stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? new DateTime(1900, 01, 01) : dtReader.GetDateTime(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                        break;

                    default:
                        stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? string.Empty : dtReader.GetString(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                        break;
                }
            }

            stringBuilder.AppendLine($"            return {Cadena.PriMin(tabla)};");
            stringBuilder.AppendLine($"        }}");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("        #endregion");

            #endregion

            stringBuilder.AppendLine($"    }}");

            stringBuilder.AppendLine($"}}");

            return $"{stringBuilder}";
        }
    }
}