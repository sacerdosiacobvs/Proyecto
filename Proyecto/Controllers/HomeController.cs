using System;
using System.Web.Mvc;
using Proyecto.Models;
using System.Net.Mail;
using System.IO;
using System.Collections.Generic;

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Credenciales.u_cedula = "";
            Credenciales.u_nombre = "";
            Credenciales.u_apellido1 = "";
            Credenciales.u_apellido2 = "";
            Credenciales.u_tipo = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login login, string submitButton)
        {
            if (ModelState.IsValid)
            {
                switch (submitButton)
                {
                    case "Registrar":
                        if (!login.Existe_Usuario())
                        {
                            login.Crear_Persona();
                            login.Crear_Usuario();
                            ViewBag.SuccessMessage = "Usuario registrado exitosamente.";
                            return View("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "ERROR. La cédula ingresada ya se encuentra en uso.";
                            return View(login);
                        }

                    case "Login":
                        if (login.Obtener_Usuario_Login())
                        {
                            login.Setear_Datos_Usuario();
                            Credenciales.u_cedula = login.cedula.ToString();
                            Credenciales.u_nombre = login.nombre;
                            Credenciales.u_apellido1 = login.apellido1;
                            Credenciales.u_apellido2 = login.apellido2;
                            Credenciales.u_tipo = login.tipo_usuario;
                            return RedirectToAction("Inicio");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "ERROR. Usuario no encontrado.";
                            return View(login);
                        }
                        
                    default:
                        return View();
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Inicio()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inicio(Inicio inicio) 
        {

            if (ModelState.IsValid)
            {

                if (inicio.Existe_Caso())
                {
                    Caso caso = new Caso();
                    caso.Cargar_Caso(inicio.id_caso);
                    ViewBag.Archivado = caso.Obtener_Archivado_Tabla();
                    return View("Informacion_Caso", caso);
                }
                else
                {
                    ViewBag.ErrorMessage = "ERROR. Expediente no encontrado.";
                    return View(inicio);
                }
                
            }
            return View(inicio);
        }

        ////////////////////////////////////////////////////////////////////// CASOS

        [HttpGet]
        public ActionResult Ingresar_Caso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ingresar_Caso(Caso caso)
        {
            if (caso.tipo_expediente == "Judicial")
            {
                ModelState.Remove("partes");
                ModelState.Remove("unidad");
            }
            else
            {
                ModelState.Remove("actora");
                ModelState.Remove("demandada");
                ModelState.Remove("valor");
                ModelState.Remove("juzgado");
            }

            if (ModelState.IsValid)
            {
                if (!caso.Existe_Caso())
                {
                    
                    
                    caso.fecha_creacion = System.DateTime.Today.ToString();
                    caso.Crear_Caso();
                    ViewBag.SuccessMessage = "Expediente registrado exitosamente.";
                    return View(caso);
                }
                
                ViewBag.ErrorMessage = "ERROR. El número de expediente ya se encuentra registrado.";
                return View(caso);

            }
            ViewBag.ErrorMessage = "ERROR. Expediente no pudo ser registrado.";

            return View(caso);
        }

        [HttpGet]
        public ActionResult Mostrar_Casos()
        {
            Caso ver_casos = new Caso();
            ViewBag.Casos = ver_casos.Obtener_Casos_Tabla();
            ViewBag.Casos_Judiciales = ver_casos.Obtener_Casos_Judiciales_Tabla();
            ViewBag.Casos_Administrativos = ver_casos.Obtener_Casos_Administrativos_Tabla();
            return View(ver_casos);
        }

        [HttpGet]
        public ActionResult Eliminar_Caso(string id_caso)
        {
            Caso caso = new Caso();
            caso.id_caso = id_caso;
            ViewBag.Id = id_caso;
            return View(caso);
        }

        [HttpPost]
        public ActionResult Eliminar_Caso(Caso caso)
        {
            caso.Eliminar_Caso();
            return RedirectToAction("Mostrar_Casos");
        }


        [HttpGet]
        public ActionResult Modificar_Caso(string id_caso)
        {
            Caso caso = new Caso();
            caso.Cargar_Caso(id_caso);
            return View(caso);
        }

        [HttpPost]
        public ActionResult Modificar_Caso(Caso caso)
        {
            if (caso.tipo_expediente == "Judicial")
            {
                ModelState.Remove("partes");
                ModelState.Remove("unidad");
            }
            else
            {
                ModelState.Remove("actora");
                ModelState.Remove("demandada");
                ModelState.Remove("valor");
                ModelState.Remove("juzgado");
            }

            if (ModelState.IsValid)
            {
                caso.Modificar_Caso();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Caso modificado exitosamente.";
                return View(caso);
            }
            else
                return View(caso);
        }

        [HttpGet]
        public ActionResult Informacion_Caso(string id_caso)
        {
            Caso caso = new Caso();
            caso.Cargar_Caso(id_caso);
            ViewBag.Archivado = caso.Obtener_Archivado_Tabla();
            return View(caso);
        }


        /// //////////////////////// ///////////////////////////////////////// VER DOCUMENTO


        [HttpGet]
        public ActionResult Ver_Archivo(int id_archivado)
        {
            Archivado archivado = new Archivado();
            archivado.Cargar_Archivado(id_archivado);
            return File(archivado.documento.ToString(), "application/pdf");

        }

        ////////////////////////////////////////////////////////////////////// INFORMES


        [HttpGet]
        public ActionResult Ingresar_Informe(string id)
        {
            if (id == null)
            {
                Informe informe = new Informe();
                ViewBag.Parametros = false;
                return View(informe);
            }
            else
            {
                Informe informe = new Informe();
                informe.id_caso = id;
                ViewBag.Parametros = true;
                return View(informe);
            }
            
        }

        [HttpPost]
        public ActionResult Ingresar_Informe(Informe informe)
        {
            if (ModelState.IsValid)
            {
                if (informe.Existe_Caso())
                {
                    informe.Crear_Informe();
                    ViewBag.SuccessMessage = "Informe registrado exitosamente.";
                    return View(informe);
                }
                else
                {
                    ViewBag.ErrorMessage = "ERROR. No existe el expediente número " + informe.id_caso + ".";
                    return View(informe);
                }
                
            }
            ViewBag.ErrorMessage = "ERROR. Informe no pudo ser registrado.";
            return View(informe);
        }

        [HttpGet]
        public ActionResult Mostrar_Informes()
        {
            Informe informes = new Informe();
            ViewBag.Informes = informes.Obtener_Informes_Tabla();
            return View(informes);
        }

        [HttpGet]
        public ActionResult Eliminar_Informe(int id_informe)
        {
            Informe informe = new Informe();
            informe.Cargar_Informe(id_informe);
            ViewBag.Id = informe.id_caso;
            return View();
        }

        [HttpPost]
        public ActionResult Eliminar_Informe(Informe informe)
        {
            informe.Eliminar_Informe();
            return RedirectToAction("Mostrar_Informes");
        }

        [HttpGet]
        public ActionResult Modificar_Informe(int id)
        {
            Informe informe = new Informe();
            informe.Cargar_Informe(id);
            return View(informe);
        }

        [HttpPost]
        public ActionResult Modificar_Informe(Informe informe)
        {
            if (ModelState.IsValid)
            {
                informe.Modificar_Informe();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Informe modificado exitosamente.";
                return View(informe);
            }
            else
                return View(informe);
        }

        ////////////////////////////////////////////////////////////////////// USUARIOS


        [HttpGet]
        public ActionResult Mostrar_Usuarios()
        {
            Login usuarios = new Login();
            ViewBag.Usuarios = usuarios.Obtener_Todos_Usuarios();
            return View(usuarios);
        }

        [HttpGet]
        public ActionResult Eliminar_Usuario(int id)
        {
            Login usuarios = new Login();
            usuarios.id_persona = id;
            usuarios.Set_Nombre();
            
            ViewBag.Cedula = usuarios.cedula;
            return View(usuarios);
        }

        [HttpPost]
        public ActionResult Eliminar_Usuario(Login usuarios)
        {
            usuarios.Eliminar_Usuario();
            return RedirectToAction("Mostrar_Usuarios");
        }

        [HttpGet]
        public ActionResult Modificar_Usuario(int id_persona, int cedula, string nombre, string apellido1, string apellido2, string tipo_usuario)
        {
            Login modificar_usuario = new Login();
            modificar_usuario.id_persona = id_persona;
            modificar_usuario.cedula = cedula;
            modificar_usuario.nombre = nombre;
            modificar_usuario.apellido1 = apellido1;
            modificar_usuario.apellido2 = apellido2;
            modificar_usuario.tipo_usuario = tipo_usuario;
            return View(modificar_usuario);
        }

        [HttpPost]
        public ActionResult Modificar_Usuario(Login modificar_usuario)
        {
            if (ModelState.IsValid)
            {
                modificar_usuario.Modificar_Usuario();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Usuario modificado exitosamente.";
                return View(modificar_usuario);
            }
            else
                return View(modificar_usuario);
        }

        [HttpGet]
        public ActionResult Ingresar_Usuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ingresar_Usuario(Login usuario)
        {
            if (ModelState.IsValid)
            {
                if (!usuario.Existe_Usuario())
                {
                    usuario.Crear_Persona();
                    usuario.Crear_Usuario();
                    ViewBag.SuccessMessage = "Usuario registrado exitosamente.";
                    return View("Ingresar_Usuario");
                }
                else
                {
                    ViewBag.ErrorMessage = "Error. Cédula ya se encuentra en uso.";
                    return View(usuario);
                }
            }

            ViewBag.ErrorMessage = "ERROR. Usuario no pudo ser registrado.";
            return View(usuario);

        }
        /////////////////////////////////////////////////////////////// ARCHIVADO

        [HttpGet]
        public ActionResult Ingresar_Archivado(string id)
        {
            if (id == null)
            {
                Archivado archivado = new Archivado();
                ViewBag.Parametros = false;
                return View(archivado);
            }
            else
            {
                Archivado archivado = new Archivado();
                archivado.id_caso = id;
                ViewBag.Parametros = true;
                return View(archivado);
            }
            
        }

        [HttpPost]
        public ActionResult Ingresar_Archivado(Archivado archivado)
        {

            if (ModelState.IsValid)
            {
                if (!archivado.Existe_Archivado())
                {
                    if (archivado.Existe_Caso())
                    {

                        if (archivado.archivo.ContentLength > 0)
                        {
                            var folder = "c:/documentos/";
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }

                            var fileName = Path.GetFileName(archivado.archivo.FileName);
                            var path = Path.Combine("c:\\documentos\\", fileName);
                            archivado.archivo.SaveAs(path);

                            archivado.documento = "c:/documentos/" + fileName;

                        }

                        archivado.Crear_Archivado();
                        ViewBag.SuccessMessage = "Archivado registrado exitosamente.";
                        return View(archivado);
                    }
                    ViewBag.ErrorMessage = "ERROR. No existe el expediente número " + archivado.id_caso + ".";
                    return View(archivado);
                }
                ViewBag.ErrorMessage = "ERROR. El archivado del expediente número "+archivado.id_caso+" ya se encuentra registrado.";
                return View(archivado);
            }
            ViewBag.ErrorMessage = "ERROR. Archivado no pudo ser registrado.";

            return View(archivado);
        }

        [HttpGet]
        public ActionResult Mostrar_Archivados()
        {
            Archivado archivado = new Archivado();
            ViewBag.Archivados = archivado.Obtener_Archivados_Tabla();
            return View(archivado);
        }

        [HttpGet]
        public ActionResult Eliminar_Archivado(string id)
        {
            Archivado archivado = new Archivado();
            archivado.id_caso = id;
            ViewBag.Id = id;
            return View(archivado);
        }

        [HttpPost]
        public ActionResult Eliminar_Archivado(Archivado archivado)
        {
            archivado.Eliminar_Archivado(archivado.id_caso);
            return RedirectToAction("Mostrar_Archivados");
        }

        [HttpGet]
        public ActionResult Modificar_Archivado(int id)
        {
            Archivado archivado = new Archivado();
            archivado.Cargar_Archivado(id);
            return View(archivado);
        }

        [HttpPost]
        public ActionResult Modificar_Archivado(Archivado archivado)
        {
            if (ModelState.IsValid)
            {
                archivado.Modificar_Archivado();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Archivado modificado exitosamente.";
                return View(archivado);
            }
            else
                return View(archivado);
        }

        /////////////////////////////////////////////////////////////// CORREOS

        [HttpGet]
        public ActionResult Enviar_Correo(int id)
        {
            Correo correo = new Correo();
            correo.id_informe = id;
            ViewBag.Contactos= correo.Obtener_Contactos();
            return View(correo);
        }

        [HttpPost]
        public ActionResult Enviar_Correo(Correo correo)
        {
            ViewBag.Contactos = correo.Obtener_Contactos();

            if (ModelState.IsValid)
            {
                try
                {

                    MailMessage correo_msm = new MailMessage();
                    correo_msm.From = new MailAddress("santabarbara.sistema.legal@gmail.com"); //correo propio a usar
                    correo_msm.To.Add(correo.destino);
                    correo_msm.Subject = correo.asunto;
                    correo_msm.Body = correo.texto;
                    correo_msm.IsBodyHtml = true;
                    correo_msm.Priority = MailPriority.Normal;

                    if (correo.archivo != null)
                    {
                        string fileName = Path.GetFileName(correo.archivo.FileName);
                        correo_msm.Attachments.Add(new Attachment(correo.archivo.InputStream, fileName));
                    }


                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Timeout = 20000;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaCorreo = "santabarbara.sistema.legal@gmail.com";
                    string sPasswordCorreo = "sbsl2021";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaCorreo, sPasswordCorreo);

                    
                    smtp.Send(correo_msm);
                    correo.Cambiar_Estado_Informe();
                    ViewBag.SuccessMessage = "Correo enviado exitosamente.";
                    return View();

                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e.Message;
                    return View();
                }
            }
            ViewBag.ErrorMessage = "ERROR. El correo no pudo ser enviado.";
            return View();
        }

        /////////////////////////////////////////////////////////////////////////////// CALENDARIO

        [HttpGet]
        public ActionResult Calendario()
        {
            Informe informes = new Informe();

            ViewBag.Lista = informes.Obtener_Informes_Calendario();
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////// CONTACTO

        [HttpGet]
        public ActionResult Ingresar_Contacto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ingresar_Contacto(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                contacto.Crear_Persona();
                contacto.Crear_Contacto();
                ViewBag.SuccessMessage = "Contacto registrado exitosamente.";
                return View(contacto);

            }
            ViewBag.ErrorMessage = "ERROR. El contacto no pudo ser registrado.";
            return View(contacto);
        }

        [HttpGet]
        public ActionResult Mostrar_Contactos()
        {
            Contacto contacto = new Contacto();
            ViewBag.Contactos = contacto.Obtener_Contactos_Tabla();
            return View(contacto);
        }

        [HttpGet]
        public ActionResult Modificar_Contacto(int id)
        {

            Contacto contacto = new Contacto();
            contacto.Cargar_Contacto(id);
            return View(contacto);
        }

        [HttpPost]
        public ActionResult Modificar_Contacto(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                contacto.Modificar_Contacto();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Contacto modificado exitosamente.";
                return View(contacto);
            }
            else
                return View(contacto);
        }

        [HttpGet]
        public ActionResult Eliminar_Contacto(int id)
        {
            Contacto contacto = new Contacto();
            contacto.Cargar_Contacto(id);
            ViewBag.Nombre = contacto.nombre;
            return View(contacto);
        }

        [HttpPost]
        public ActionResult Eliminar_Contacto(Contacto contacto)
        {
            contacto.Eliminar_Contacto();
            return RedirectToAction("Mostrar_Archivados");
        }

        ///////////////////////////////////////////////////////////// ENVIAR DATOS DE USUARIO POR CORREO

        [HttpGet]
        public ActionResult Correo_Usuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Correo_Usuario(Correo_Usuario correo_usuario)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    correo_usuario.Cargar_Datos(); //carga datos del usuario

                    MailMessage correo_msm = new MailMessage();
                    correo_msm.From = new MailAddress("santabarbara.sistema.legal@gmail.com"); //correo propio a usar
                    correo_msm.To.Add(correo_usuario.destino);
                    correo_msm.Subject = "Recuperación de datos";
                    correo_msm.IsBodyHtml = true;
                    correo_msm.Priority = MailPriority.Normal;
                    correo_msm.Body = correo_usuario.texto;


                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Timeout = 20000;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaCorreo = "santabarbara.sistema.legal@gmail.com";
                    string sPasswordCorreo = "sbsl2021";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaCorreo, sPasswordCorreo);


                    smtp.Send(correo_msm);
                    ViewBag.SuccessMessage = "Correo enviado, revise su carpeta de entrada.";
                    return View();

                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e.Message;
                    return View();
                }
            }
            ViewBag.ErrorMessage = "ERROR. El correo no pudo ser enviado.";
            return View();
        }

    }
}