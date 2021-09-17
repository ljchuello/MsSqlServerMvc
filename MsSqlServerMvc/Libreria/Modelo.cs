using System;
using System.Collections.Generic;
using System.Text;

namespace MsSqlServerMvc.Libreria
{
    public class Modelo
    {
        public string Generar(string tabla, List<Sql.Campos> campos)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"// Mo{tabla}.cs");
            stringBuilder.AppendLine($"// Clase generada por");
            stringBuilder.AppendLine($"// Leonardo Chuello");
            stringBuilder.AppendLine($"// {DateTime.Now:yyyy-MM-dd}");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"using System;");
            stringBuilder.AppendLine($"");
            stringBuilder.AppendLine($"namespace Modelo");
            stringBuilder.AppendLine($"{{");
            stringBuilder.AppendLine($"    public class Mo{tabla}");
            stringBuilder.AppendLine($"    {{");

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
                        stringBuilder.AppendLine($"        public NOPE {campo.Nombre} {{ set; get; }} = NOPE");
                        break;
                }
            }

            stringBuilder.AppendLine($"    }}");
            stringBuilder.AppendLine($"}}");

            return $"{stringBuilder}";
        }
    }
}