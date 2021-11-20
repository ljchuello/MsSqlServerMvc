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