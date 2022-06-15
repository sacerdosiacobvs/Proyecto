using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Informe
    {

        [Required(ErrorMessage = "Ingresar el tipo de caso")]
        public string id_caso { get; set; }
        [Required(ErrorMessage = "Ingresar el tipo de informe")]
        public string tipo_informe { get; set; }
        [Required(ErrorMessage = "Ingresar la descripcion")]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "Ingresar una fecha límite")]
        public string fecha_respuesta { get; set; }

        public int id_informe { get; set; }
        public string estado { get; set; }


        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public void Crear_Informe()
        {

            id_informe = Contar_Informes();

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "insert into informes (ID_INFORME, ID_CASO, TIPO_INFORME, DESCRIPCION, FECHA_RESPUESTA, ESTADO) values (" + id_informe + ",'" + id_caso + "', '"+tipo_informe+"', '" + descripcion + "', '" + fecha_respuesta + "', 'PENDIENTE')";
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public int Contar_Informes()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("select ID_INFORME from informes", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return (x + 1);
        }

        public DataSet Obtener_Informes_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("select ID_INFORME, ID_CASO, DESCRIPCION, DATE_FORMAT(FECHA_RESPUESTA, '%d/%m/%Y') as FECHA_RESPUESTA, ESTADO, TIPO_INFORME from informes;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public DataSet Obtener_Informes_Calendario()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("select ID_INFORME, ID_CASO, DESCRIPCION, CONVERT(FECHA_RESPUESTA, CHAR) AS FECHA_RESPUESTA, ESTADO from informes where ESTADO='PENDIENTE';", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public void Eliminar_Informe()
        {
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "delete from informes where ID_INFORME=" + id_informe;
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void Cargar_Informe(int id_informe_buscar)
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT ID_CASO, TIPO_INFORME, DESCRIPCION, CONVERT(FECHA_RESPUESTA, CHAR) AS FECHA_RESPUESTA, ESTADO FROM informes WHERE ID_INFORME=" + id_informe_buscar, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            id_informe = id_informe_buscar;
            id_caso = ds.Tables[0].Rows[0]["ID_CASO"].ToString(); ;
            tipo_informe = ds.Tables[0].Rows[0]["TIPO_INFORME"].ToString();
            descripcion = ds.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            fecha_respuesta = ds.Tables[0].Rows[0]["FECHA_RESPUESTA"].ToString();
            estado = ds.Tables[0].Rows[0]["ESTADO"].ToString();
        }

        public void Modificar_Informe()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "UPDATE informes SET TIPO_INFORME='" + tipo_informe + "', DESCRIPCION='" + descripcion + "', FECHA_RESPUESTA ='" + fecha_respuesta + "', ESTADO = '" + estado + "' where ID_INFORME=" + id_informe;
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
        }

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