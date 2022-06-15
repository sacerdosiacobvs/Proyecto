using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Login
    {
        public int id_persona { get; set; }

        [Required(ErrorMessage = "Ingrese su cédula.")]
        public int cedula { get; set; }
        [Required(ErrorMessage = "Ingrese su contraseña.")]
        public string password { get; set; }

        //[Required(ErrorMessage = "Ingrese su nombre")]
        public string nombre { get; set; }
        //[Required(ErrorMessage = "Ingrese su nombre")]
        public string apellido1 { get; set; }
        //[Required(ErrorMessage = "Ingrese su nombre")]
        public string apellido2 { get; set; }
        //[Required(ErrorMessage = "Ingrese el tipo")]
        public string tipo_usuario { get; set; }

        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";


        public void Crear_Usuario()
        {

            //int id_persona = Contar_Personas();

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "INSERT INTO usuarios (CEDULA, ID_PERSONA, PASSWORD, TIPO_USUARIO) VALUES(" + cedula + ", " + id_persona + ", '" + password + "', 'GUEST')";
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void Crear_Persona()
        {

            id_persona = Contar_Personas();

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "INSERT INTO personas (ID_PERSONA, NOMBRE, APELLIDO1, APELLIDO2) VALUES(" + id_persona + ", '" + nombre + "', '" + apellido1 + "', '" + apellido2 + "')";
            comando.ExecuteNonQuery();

            conexion.Close();
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

        public Boolean Existe_Usuario()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM usuarios WHERE CEDULA=" + cedula, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }


        public Boolean Obtener_Usuario_Login()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM usuarios WHERE CEDULA=" + cedula + " and PASSWORD='" + password + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void Setear_Datos_Usuario()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM usuarios JOIN personas WHERE usuarios.ID_PERSONA = personas.ID_PERSONA AND usuarios.CEDULA =" + cedula, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            nombre = (ds.Tables[0].Rows[0]["NOMBRE"]).ToString();
            apellido1 = (ds.Tables[0].Rows[0]["APELLIDO1"]).ToString();
            apellido2 = (ds.Tables[0].Rows[0]["APELLIDO2"]).ToString();
            tipo_usuario = (ds.Tables[0].Rows[0]["TIPO_USUARIO"]).ToString();

        }


        public DataSet Obtener_Todos_Usuarios()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM usuarios JOIN personas WHERE usuarios.ID_PERSONA = personas.ID_PERSONA;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        //int cedula, string nombre, string tipo, string password

        public void Modificar_Usuario() // Y PERSONA
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "UPDATE usuarios SET PASSWORD='" + password +"', TIPO_USUARIO ='" + tipo_usuario + "' where CEDULA=" + cedula;
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            SQL = "UPDATE personas SET NOMBRE='" + nombre + "', APELLIDO1 ='" + apellido1 + "', APELLIDO2 = '"+ apellido2 + "' where ID_PERSONA=" + id_persona;
            sql = new MySqlCommand(SQL, con);
            da = new MySqlDataAdapter(sql);
            da.Fill(ds);

        }

        public void Set_Nombre()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT CEDULA FROM usuarios WHERE ID_PERSONA=" + id_persona, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            cedula = Convert.ToInt32(ds.Tables[0].Rows[0]["CEDULA"]);

        }

        public void Eliminar_Usuario()
        {
            //MySqlConnection conexion = new MySqlConnection(connectionString);
            //conexion.Open();

            //MySqlCommand comando = new MySqlCommand();
            //comando.Connection = conexion;
            //comando.CommandText = "DELETE FROM personas WHERE ID_PERSONA=" + id_persona;
            //comando.ExecuteNonQuery();

            //conexion.Close();

            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "DELETE FROM personas WHERE ID_PERSONA=" + id_persona;
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

        }

    }
}