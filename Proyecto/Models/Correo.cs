using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;

namespace Proyecto.Models
{
    public class Correo
    {
        [Required(ErrorMessage = "Seleccione el destinatario")]
        public string destino { get; set; }

        public string asunto { get; set; }

        public string texto { get; set; }

        public HttpPostedFileBase archivo { get; set; }

        [Required(ErrorMessage = "Seleccione el id informe")]
        public int id_informe { get; set; }



        string connectionString = "Server=localhost;Database=casos_legales;Uid=root;Pwd=123456;convert zero datetime=True;";

        public DataSet Obtener_Contactos()
        {
            DataSet ds = new DataSet();
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand sql = new MySqlCommand("SELECT * FROM contactos JOIN personas WHERE contactos.ID_PERSONA = personas.ID_PERSONA;", con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            da.Fill(ds);

            return ds;
        }

        public void Cambiar_Estado_Informe()
        {
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "UPDATE informes SET ESTADO='ENTREGADO' where ID_INFORME=" + id_informe;
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        
    }
}