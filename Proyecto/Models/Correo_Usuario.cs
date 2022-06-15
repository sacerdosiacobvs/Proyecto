using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Correo_Usuario
    {
        [Required(ErrorMessage = "Ingrese su cédula")]
        public int cedula { get; set; }

        [Required(ErrorMessage = "Ingrese su correo electrónico")]
        public string destino { get; set; }

        public string texto { get; set; }

        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public void Cargar_Datos()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM usuarios JOIN personas WHERE usuarios.ID_PERSONA = personas.ID_PERSONA AND usuarios.CEDULA=" + cedula, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            texto = "Cédula de usuario: " + ds.Tables[0].Rows[0]["CEDULA"].ToString() +
                "<br />Nombre completo: " + ds.Tables[0].Rows[0]["NOMBRE"].ToString()+" " + ds.Tables[0].Rows[0]["APELLIDO1"].ToString()+" " + ds.Tables[0].Rows[0]["APELLIDO2"].ToString()+
                "<br />Contraseña: " + ds.Tables[0].Rows[0]["PASSWORD"].ToString()+
                "<br />Tipo de usuario: " + ds.Tables[0].Rows[0]["TIPO_USUARIO"].ToString();
        }
    }
}