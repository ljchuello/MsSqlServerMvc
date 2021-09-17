using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MsSqlServerMvc.Libreria
{
    public class UControl
    {
        public static void EvitarDobleEnvioButton(Page pagina, Button button)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            stringBuilder.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            stringBuilder.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            stringBuilder.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            //stringBuilder.Append($"this.value = 'Procesando!.';");
            stringBuilder.Append("this.disabled = true;");
            stringBuilder.Append(pagina.ClientScript.GetPostBackEventReference(button, string.Empty) + ";");
            stringBuilder.Append("return false;");

            button.Attributes.Add("onclick", stringBuilder.ToString());
        }

        public static void EvitarDobleEnvioButton(Page pagina, HtmlButton button)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            stringBuilder.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            stringBuilder.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            stringBuilder.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            //stringBuilder.Append($"this.innerText  = 'Procesando!.';");
            stringBuilder.Append("this.disabled = true;");
            stringBuilder.Append(pagina.ClientScript.GetPostBackEventReference(button, string.Empty) + ";");
            stringBuilder.Append("return false;");

            button.Attributes.Add("onclick", stringBuilder.ToString());
        }

        public static void SetFocus(Page pagina, TextBox textBox)
        {
            textBox.Focus();
            textBox.Attributes.Add("onfocus", "this.select()");
        }
    }
}