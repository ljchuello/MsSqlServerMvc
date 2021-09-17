using System;
using System.Web.UI;

namespace MsSqlServerMvc.Libreria
{
    public class Javascript
    {
        public static void ResizeTxt(Page page, string txtClientId)
        {
            try
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), $"{Guid.NewGuid()}", $"M.textareaAutoResize($('#{txtClientId}'));", true);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}