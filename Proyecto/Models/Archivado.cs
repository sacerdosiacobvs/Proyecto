using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Proyecto.Models
{
    public class Archivado
    {
        public int id_archivado { get; set; }

        [Required(ErrorMessage = "Ingresar el caso")]
        public string id_caso { get; set; }
        [Required(ErrorMessage = "Ingresar la ubicación")]
        public string ubicacion { get; set; }
        [Required(ErrorMessage = "Ingresar el tipo de archivo")]
        public string tipo_archivo { get; set; }

        public string estado { get; set; }

        [Required(ErrorMessage = "Ingresar el documento")]
        public HttpPostedFileBase archivo { get; set; }

        public string documento { get; set; }


        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public void Crear_Archivado()
        {

            id_archivado = Contar_Archivados();

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "insert into archivos (ID_ARCHIVO, ID_CASO, UBICACION, TIPO_ARCHIVO, ESTADO, DOCUMENTO) values (" + id_archivado + ", '" + id_caso + "', '" + ubicacion + "', '" + tipo_archivo + "', '" + estado + "', '" + documento + "')";
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public int Contar_Archivados()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("select ID_ARCHIVO from archivos", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return (x + 1);
        }

        public DataSet Obtener_Archivados_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("select * from archivos;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public Boolean Existe_Archivado()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM archivos WHERE ID_CASO='" + id_caso + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
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

        public void Eliminar_Archivado(string id)
        {
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "delete from archivos where ID_CASO='" + id + "'";
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void Modificar_Archivado()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "UPDATE archivos SET UBICACION='" + ubicacion + "', TIPO_ARCHIVO='" + tipo_archivo + "', ESTADO ='" + estado + "' where ID_ARCHIVO=" + id_archivado;
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
        }

        public void Cargar_Archivado(int id_archivo_buscar)
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM archivos WHERE ID_ARCHIVO=" + id_archivo_buscar, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            id_archivado = id_archivo_buscar;
            id_caso = ds.Tables[0].Rows[0]["ID_CASO"].ToString(); ;
            ubicacion = ds.Tables[0].Rows[0]["UBICACION"].ToString(); ;
            tipo_archivo = ds.Tables[0].Rows[0]["TIPO_ARCHIVO"].ToString();
            estado = ds.Tables[0].Rows[0]["ESTADO"].ToString();
            documento = ds.Tables[0].Rows[0]["DOCUMENTO"].ToString();

        }

    }
}