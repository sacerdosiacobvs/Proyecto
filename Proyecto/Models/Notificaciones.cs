using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Notificaciones
    {
        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public string Cantidad_Notificaciones()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM informes WHERE (FECHA_RESPUESTA between CURDATE() AND CURDATE()+20) AND ESTADO='PENDIENTE';", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return x.ToString();

        }

        public DataSet Obtener_Notificaciones_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM informes WHERE (FECHA_RESPUESTA between CURDATE() AND CURDATE()+20) AND ESTADO='PENDIENTE';", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            return ds;
        }



    }
}