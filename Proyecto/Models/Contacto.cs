using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Contacto
    {
        public int id_persona { get; set; }

        public int id_contacto { get; set; }

        [Required(ErrorMessage = "Ingresar un nombre")]
        public string nombre { get; set; }

        //[Required(ErrorMessage = "Ingrese su nombre")]
        public string apellido1 { get; set; }
        //[Required(ErrorMessage = "Ingrese su nombre")]
        public string apellido2 { get; set; }

        [Required(ErrorMessage = "Seleccionar la dirección de correo")]
        public string correo { get; set; }



        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public void Crear_Contacto()
        {

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;

            id_contacto = Contar_Contactos();

            comando.CommandText = "INSERT INTO contactos (ID_CONTACTO, ID_PERSONA, CORREO) VALUES(" + id_contacto + ", " + id_persona + ", '" + correo + "')";
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void Crear_Persona()
        {

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;

            id_persona = Contar_Personas();
            comando.CommandText = "INSERT INTO personas (ID_PERSONA, NOMBRE, APELLIDO1, APELLIDO2) VALUES(" + id_persona + ", '" + nombre + "', '"+apellido1+"', '"+apellido2+"')";
            comando.ExecuteNonQuery();

            conexion.Close();
        }


        //public DataSet Obtener_Contactos_Tabla()
        //{
        //    DataSet ds = new DataSet();
        //    MySqlConnection con = new MySqlConnection(connectionString);
        //    MySqlCommand sql = new MySqlCommand("SELECT * FROM contactos JOIN personas WHERE contactos.ID_PERSONA = personas.ID_PERSONA;", con);
        //    MySqlDataAdapter da = new MySqlDataAdapter(sql);
        //    da.Fill(ds);
        //    return ds;
        //}

        public void Eliminar_Contacto()
        {
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "DELETE FROM personas WHERE ID_PERSONA=" + id_persona;
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void Modificar_Contacto() //??
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "UPDATE contactos SET CORREO='" + correo + "' where ID_CONTACTO=" + id_contacto;
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            SQL = "UPDATE personas SET NOMBRE='" + nombre + "', APELLIDO1='"+apellido1+"', APELLIDO2='"+apellido2+"' where ID_PERSONA=" + id_persona;
            sql = new MySqlCommand(SQL, con);
            da = new MySqlDataAdapter(sql);
            da.Fill(ds);
        }

        public void Cargar_Contacto(int id_persona_cargar)
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM contactos JOIN personas WHERE contactos.ID_PERSONA = personas.ID_PERSONA AND contactos.ID_PERSONA=" + id_persona_cargar, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            id_persona = id_persona_cargar;
            id_contacto = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_CONTACTO"]);
            nombre = ds.Tables[0].Rows[0]["NOMBRE"].ToString(); 
            correo = ds.Tables[0].Rows[0]["CORREO"].ToString();

        }

        public int Contar_Personas()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM personas", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return (x + 1);
        }

        public int Contar_Contactos()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM contactos", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return (x + 1);
        }

    }
}