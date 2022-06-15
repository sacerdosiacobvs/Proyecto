using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Inicio
    {
        [Required(ErrorMessage = "Ingresar el número de expediente")]
        public string id_caso { get; set; }

        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public Boolean Existe_Caso()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos WHERE ID_CASO='" + id_caso + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}