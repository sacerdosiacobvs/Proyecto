using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Caso
    {
        [Required(ErrorMessage = "Ingresar el número de expediente.")]
        public string id_caso { get; set; }
        [Required(ErrorMessage = "Seleccione el estado.")]
        public string estado { get; set; }
        [Required(ErrorMessage = "Seleccione el tipo de proceso.")]
        public string tipo_proceso { get; set; }
        [Required(ErrorMessage = "Ingrese la descripcion.")]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "Seleccione el tipo de expediente.")]
        public string tipo_expediente { get; set; }



        public string fecha_creacion { get; set; } //fecha de creacion

        public int cedula { get; set; } //usuario que registra o modifica el expediente de caso

        /// JUDICIAL
        [Required(ErrorMessage = "Ingrese la persona actora.")]
        public string actora { get; set; }
        [Required(ErrorMessage = "Ingrese la persona demandada.")]
        public string demandada { get; set; }
        [Required(ErrorMessage = "Ingrese el valor.")]
        public string valor { get; set; }
        [Required(ErrorMessage = "Ingrese el juzgado.")]
        public string juzgado { get; set; }

        /// ADMINISTRATIVO
        [Required(ErrorMessage = "Ingrese las partes.")]
        public string partes { get; set; }
        [Required(ErrorMessage = "Ingrese la unidad.")]
        public string unidad { get; set; }


        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public void Crear_Caso()
        {


            
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            
            comando.CommandText = "INSERT INTO casos (ID_CASO, ESTADO, TIPO_PROCESO, DESCRIPCION, TIPO_EXPEDIENTE, FECHA_CREACION, CEDULA) VALUES('" + id_caso + "', '" + estado + "', '" + tipo_proceso + "', '" + descripcion + "', '" + tipo_expediente + "', '" + fecha_creacion + "', " + Convert.ToInt32(Credenciales.u_cedula) + ")";
            comando.ExecuteNonQuery();

            if (tipo_expediente == "Judicial")
            {

                int valor_int = Convert.ToInt32(valor.ToString());
                comando.CommandText = "INSERT INTO exp_judiciales (ID_CASO, ACTORA, DEMANDADA, VALOR, JUZGADO) VALUES('" + id_caso + "', '" + actora + "', '" + demandada + "', " + valor_int + ", '" + juzgado + "')";
                comando.ExecuteNonQuery();
            }
            else
            {
                if(tipo_expediente == "Administrativo")
                {
                    comando.CommandText = "INSERT INTO exp_administrativos (ID_CASO, PARTES, UNIDAD) VALUES('" + id_caso + "', '" + partes + "', '" + unidad + "')";
                    comando.ExecuteNonQuery();
                }
            }


            conexion.Close();
        }



        public int Contar_Casos()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            int x = ds.Tables[0].Rows.Count;

            return (x + 1);
        }


        public DataSet Obtener_Casos_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public DataSet Obtener_Casos_Judiciales_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos JOIN exp_judiciales WHERE casos.ID_CASO = exp_judiciales.ID_CASO;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public DataSet Obtener_Casos_Administrativos_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos JOIN exp_administrativos WHERE casos.ID_CASO = exp_administrativos.ID_CASO;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public void Cargar_Caso(string id_caso_buscar)
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM casos WHERE ID_CASO='" + id_caso_buscar+"'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            id_caso = id_caso_buscar;
            estado = ds.Tables[0].Rows[0]["ESTADO"].ToString();
            tipo_proceso = ds.Tables[0].Rows[0]["TIPO_PROCESO"].ToString();
            descripcion = ds.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            tipo_expediente = ds.Tables[0].Rows[0]["TIPO_EXPEDIENTE"].ToString();
            if (ds.Tables[0].Rows[0]["CEDULA"] == DBNull.Value)
            {
                cedula = 0;
            }
            else
            {
                cedula = Convert.ToInt32(ds.Tables[0].Rows[0]["CEDULA"]);
            }
            

            if (tipo_expediente == "Judicial")
            {
                DataSet ds_2 = new DataSet();
                MySqlConnection con_2 = new MySqlConnection(connectionString);
                MySqlCommand sql_2 = new MySqlCommand("SELECT * FROM exp_judiciales WHERE ID_CASO='" + id_caso_buscar + "'", con_2);
                MySqlDataAdapter da_2 = new MySqlDataAdapter(sql_2);
                da_2.Fill(ds_2);



                actora = ds_2.Tables[0].Rows[0]["ACTORA"].ToString();
                demandada = ds_2.Tables[0].Rows[0]["DEMANDADA"].ToString();
                valor = ds_2.Tables[0].Rows[0]["VALOR"].ToString();
                juzgado = ds_2.Tables[0].Rows[0]["JUZGADO"].ToString();
            }
            else
            {
                if(tipo_expediente == "Administrativo")
                {
                    DataSet ds_2 = new DataSet();
                    MySqlConnection con_2 = new MySqlConnection(connectionString);
                    MySqlCommand sql_2 = new MySqlCommand("SELECT * FROM exp_administrativos WHERE ID_CASO='" + id_caso_buscar + "'", con_2);
                    MySqlDataAdapter da_2 = new MySqlDataAdapter(sql_2);
                    da_2.Fill(ds_2);

                    partes = ds_2.Tables[0].Rows[0]["PARTES"].ToString();
                    unidad = ds_2.Tables[0].Rows[0]["UNIDAD"].ToString();
                }
            }
        }


        public void Eliminar_Caso()
        {
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "DELETE FROM casos WHERE ID_CASO='" + id_caso + "'";
            comando.ExecuteNonQuery();

            conexion.Close();
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

        public void Modificar_Caso()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            string SQL = "UPDATE casos SET ESTADO='" + estado + "', TIPO_PROCESO='" + tipo_proceso + "', DESCRIPCION ='" + descripcion + "', TIPO_EXPEDIENTE = '" + tipo_expediente + "', CEDULA=" + Convert.ToInt32(Credenciales.u_cedula) + " where ID_CASO='" + id_caso + "'";
            MySqlCommand sql = new MySqlCommand(SQL, con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            if (tipo_expediente == "Judicial") // SI EL TIPO ES JUDICIAL
            {
                if (Existe_Judicial())
                {
                    DataSet ds_2 = new DataSet();
                    MySqlConnection con_2 = new MySqlConnection(connectionString);
                    string SQL_2 = "UPDATE exp_judiciales SET ACTORA='" + actora + "', DEMANDADA='" + demandada + "', VALOR ='" + valor + "', JUZGADO = '" + juzgado + "' where ID_CASO='" + id_caso + "'";
                    MySqlCommand sql_2 = new MySqlCommand(SQL_2, con);
                    MySqlDataAdapter da_2 = new MySqlDataAdapter(sql_2);
                    da_2.Fill(ds_2);
                }
                else
                {
                    int valor_int = Convert.ToInt32(valor.ToString());

                    MySqlConnection conexion = new MySqlConnection(connectionString);
                    conexion.Open();

                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    
                    comando.CommandText = "INSERT INTO exp_judiciales (ID_CASO, ACTORA, DEMANDADA, VALOR, JUZGADO) VALUES('" + id_caso + "', '" + actora + "', '" + demandada + "', " + valor_int + ", '" + juzgado + "')";
                    comando.ExecuteNonQuery();
                }
                

                if (Existe_Administrativo())
                {
                    DataSet ds_borrar = new DataSet();
                    MySqlConnection con_borrar = new MySqlConnection(connectionString);
                    string SQL_borrar = "DELETE FROM exp_administrativos WHERE ID_CASO='" + id_caso + "'";
                    MySqlCommand sql_borrar = new MySqlCommand(SQL_borrar, con);
                    MySqlDataAdapter da_borrar = new MySqlDataAdapter(sql_borrar);
                    da_borrar.Fill(ds_borrar);
                }
            }
            else
            {
                if (tipo_expediente == "Administrativo") // SI EL TIPO ES ADMINISTRATIVO
                {
                    if (Existe_Judicial())
                    {
                        DataSet ds_borrar = new DataSet();
                        MySqlConnection con_borrar = new MySqlConnection(connectionString);
                        string SQL_borrar = "DELETE FROM exp_judiciales WHERE ID_CASO='" + id_caso + "'";
                        MySqlCommand sql_borrar = new MySqlCommand(SQL_borrar, con);
                        MySqlDataAdapter da_borrar = new MySqlDataAdapter(sql_borrar);
                        da_borrar.Fill(ds_borrar);
                        
                    }
                    

                    if (Existe_Administrativo())
                    {
                        DataSet ds_2 = new DataSet();
                        MySqlConnection con_2 = new MySqlConnection(connectionString);
                        string SQL_2 = "UPDATE exp_administrativos SET PARTES='" + partes + "', UNIDAD='" + unidad + "' where ID_CASO='" + id_caso + "'";
                        MySqlCommand sql_2 = new MySqlCommand(SQL_2, con);
                        MySqlDataAdapter da_2 = new MySqlDataAdapter(sql_2);
                        da_2.Fill(ds_2);
                    }
                    else
                    {

                        MySqlConnection conexion = new MySqlConnection(connectionString);
                        conexion.Open();

                        MySqlCommand comando = new MySqlCommand();
                        comando.Connection = conexion;

                        comando.CommandText = "INSERT INTO exp_administrativos (ID_CASO, PARTES, UNIDAD) VALUES('" + id_caso + "', '" + partes + "', '" + unidad + "')";
                        comando.ExecuteNonQuery();
                    }
                }
            }

        }

        public bool Existe_Judicial()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM exp_judiciales WHERE ID_CASO='" + id_caso + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool Existe_Administrativo()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM exp_administrativos WHERE ID_CASO='" + id_caso + "'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public DataSet Obtener_Archivado_Tabla()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM archivos WHERE ID_CASO='"+id_caso+"'", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);
            return ds;
        }

        public string Obtener_Nombre_Completo_Usuario()
        {
            if (cedula == 0)
            {
                return "";
            }
            else
            {
                Login login = new Login();
                login.cedula = cedula;
                login.Setear_Datos_Usuario();

                return login.nombre + " " + login.apellido1 + " " + login.apellido2;
            }
            
        }

    }
}