using System;
using System.Collections.Generic;
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
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"namespace DataCloud");
            stringBuilder.AppendLine($"{{");
            stringBuilder.AppendLine($"    // ReSharper disable once InconsistentNaming");
            stringBuilder.AppendLine($"    public class {tabla}");
            stringBuilder.AppendLine($"    {{");

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
            stringBuilder.AppendLine($"    }}");
            #endregion
            stringBuilder.AppendLine($"}}");

            return $"{stringBuilder}";
        }
    }
}