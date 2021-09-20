<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MsSqlServerMvc.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Generador MVC</title>

    <%-- jquery --%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <%-- base --%>
    <link href="/script/base/css/stylesheet.css" rel="stylesheet" />
    <script src="/script/base/js/javascript.js"></script>

    <%-- materializecss --%>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/css/materialize.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/js/materialize.min.js"></script>

    <%-- font awesome --%>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <script>
            //On Page Load
            $(function () {
                InitV2();
            });
            //On UpdatePanel Refresh
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitV2();
                    }
                });
            };
        </script>

        <%-- Tab's --%>
        <div class="row">
            <div class="col s12">
                <ul class="tabs">
                    <li class="tab"><a class="active" href="#tabPrincipal">Principal</a></li>
                    <li class="tab"><a href="#tabModelo">Modelo</a></li>
                    <li class="tab"><a href="#tabControlador">Controlador</a></li>
                    <li class="tab"><a href="#tabGranular">Granular</a></li>
                </ul>
            </div>
            <div id="tabPrincipal" class="col s12 padding5">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="col s6">
                            <div class="input-field col s12">
                                <asp:TextBox ID="txtServidor" runat="server"></asp:TextBox>
                                <label for="<%=txtServidor.ClientID%>">Servidor</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                                <label for="<%=txtUsuario.ClientID%>">Usuario</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtContrasenia" runat="server"></asp:TextBox>
                                <label for="<%=txtContrasenia.ClientID%>">Contrasenia</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtBaseDatos" runat="server"></asp:TextBox>
                                <label for="<%=txtBaseDatos.ClientID%>">Base de datos</label>
                            </div>

                            <div class="col s12">
                                <button id="btnConectar" runat="server" class="btn blue col s12" onserverclick="btnConectar_OnServerClick">
                                    Conectar
                                </button>
                            </div>

                        </div>

                        <div class="col s6">

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlTabla" runat="server"></asp:DropDownList>
                                <label for="<%=ddlTabla.ClientID%>">Tabla</label>
                            </div>

                            <div class="col s12">
                                <button id="btnGenerar" runat="server" class="btn green col s12" onserverclick="btnGenerar_OnServerClick">
                                    Generar
                                </button>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="tabModelo" class="col s12 padding5">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="input-field col s12">
                                <textarea id="txtModelo" runat="server" class="materialize-textarea"></textarea>
                                <label for="<%=txtModelo.ClientID%>">Modelo</label>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="tabControlador" class="col s12 padding5">Controlador</div>

            <div id="tabGranular" class="col s12 padding5">Granular</div>
        </div>

    </form>
</body>
</html>
