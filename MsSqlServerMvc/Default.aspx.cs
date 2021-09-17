﻿using System;
using System.Web.UI;
using MsSqlServerMvc.Libreria;

namespace MsSqlServerMvc
{
    public partial class Default : Page
    {
        private Sql _sql = new Sql();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Evitamos el doble click
                    UControl.EvitarDobleEnvioButton(this, btnConectar);
                }
            }
            catch (Exception ex)
            {
                Notificacion.Toas(this, $"Ah ocurrido un error; {ex.Message}");
                return;
            }
        }

        protected void btnConectar_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                // Servidor
                if (Cadena.Vacia(txtServidor.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar el servidor");
                    return;
                }

                // Usuario
                if (Cadena.Vacia(txtUsuario.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar el usuario");
                    return;
                }

                // Contraseña
                if (Cadena.Vacia(txtContrasenia.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar la contraseña");
                    return;
                }

                if (Cadena.Vacia(txtBaseDatos.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar el nombre de la BD");
                    return;
                }

                // Set
                _sql.Servidor = txtServidor.Text;
                _sql.Usuario = txtUsuario.Text;
                _sql.Contrasenia = txtContrasenia.Text;
                _sql.BaseDatos = txtBaseDatos.Text;

                // Probamos
                if (!_sql.Validar(this, _sql))
                {
                    Notificacion.Toas(this, $"No se ha podido establecer la conexion");
                    return;
                }

                // Conexión exitosa
                _sql.Guardar(_sql);
            }
            catch (Exception ex)
            {
                Notificacion.Toas(this, $"Ah ocurrido un error; {ex.Message}");
                return;
            }
        }
    }
}