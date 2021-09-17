using System;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class Notificacion
    {
        public static void Toas(Page page, string mensaje)
        {
            try
            {
                mensaje = mensaje.Replace("'", "&#39;");
                mensaje = mensaje.Replace("\r", "");
                mensaje = mensaje.Replace("\n", " ");
                ScriptManager.RegisterStartupScript(page, page.GetType(), $"{Guid.NewGuid()}", $"M.toast({{html: '{mensaje}'}});", true);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}