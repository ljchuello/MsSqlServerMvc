using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class Granular
    {
        private int iterator = 0;

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
                stringBuilder.AppendLine("namespace Granular");
                stringBuilder.AppendLine("{");

                stringBuilder.AppendLine($"    public class o{tabla}");
                stringBuilder.AppendLine("    {");

                #region Insert

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("      // ReSharper disable once InconsistentNaming");
                stringBuilder.AppendLine($"      public static string Insert(Mo{tabla} Mo{tabla})");
                stringBuilder.AppendLine("      {");
                stringBuilder.AppendLine("          StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine("          stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"         stringBuilder.AppendLine(\"--  Insert {tabla}\");");
                stringBuilder.AppendLine($"         stringBuilder.AppendLine(\"INSERT INTO {tabla}\");");
                stringBuilder.AppendLine($"         stringBuilder.AppendLine(\"(\");");
                iterator = 0;
                foreach (Sql.Campos row in list)
                {
                    // Incrementamos
                    iterator = iterator + 1;

                    if (list.Count != iterator)
                    {
                        stringBuilder.AppendLine($"         stringBuilder.AppendLine(\"{row.Nombre}, -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                    }
                    else
                    {
                        stringBuilder.AppendLine($"         stringBuilder.AppendLine(\"{row.Nombre} -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                    }
                }
                stringBuilder.AppendLine($"         stringBuilder.AppendLine(\") VALUES (\");");
                iterator = 0;
                foreach (Sql.Campos row in list)
                {
                    // Incrementamos
                    iterator = iterator + 1;

                    // Validamos
                    if (row.Nulo)
                    {
                        if (iterator != list.Count)
                        {
                            // Si nulo - No ultimo
                            stringBuilder.AppendLine($"         stringBuilder.AppendLine(!Cadena.Vacia(Mo{tabla}.{row.Nombre})");
                            stringBuilder.AppendLine($"             ? $\"'{{Tools.Remplazar(Mo{tabla}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\"");
                            stringBuilder.AppendLine($"             : $\"NULL, -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                        }
                        else
                        {
                            // Si nulo - Si ultimo
                            stringBuilder.AppendLine($"         stringBuilder.AppendLine($\"'{{Tools.Remplazar(Mo{tabla}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                        }
                    }
                    else
                    {
                        if (iterator != list.Count)
                        {
                            // No nulo - No ultimo
                            stringBuilder.AppendLine($"         stringBuilder.AppendLine($\"'{{Tools.Remplazar(Mo{tabla}.{row.Nombre})}})', -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                        }
                        else
                        {
                            // No nulo - Si ultimo
                            stringBuilder.AppendLine($"         stringBuilder.AppendLine($\"'{{Tools.Remplazar(Mo{tabla}.{row.Nombre})}}' -- {row.Nombre} | {row.TipoDotNet} | {row.TipoSql}\");");
                        }
                        
                    }
                }
                stringBuilder.AppendLine("          stringBuilder.AppendLine(\");\");");
                stringBuilder.AppendLine("          return stringBuilder.ToString();");
                stringBuilder.AppendLine("      }");

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