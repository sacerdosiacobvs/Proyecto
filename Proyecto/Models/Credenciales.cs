using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public static class Credenciales
    {
        public static string u_cedula
        {
            get
            {
                return HttpContext.Current.Application["u_cedula"] as string;
            }
            set
            {
                HttpContext.Current.Application["u_cedula"] = value;
            }
        }

        public static string u_nombre
        {
            get
            {
                return HttpContext.Current.Application["u_nombre"] as string;
            }
            set
            {
                HttpContext.Current.Application["u_nombre"] = value;
            }
        }

        public static string u_apellido1
        {
            get
            {
                return HttpContext.Current.Application["u_apellido1"] as string;
            }
            set
            {
                HttpContext.Current.Application["u_apellido1"] = value;
            }
        }

        public static string u_apellido2
        {
            get
            {
                return HttpContext.Current.Application["u_apellido2"] as string;
            }
            set
            {
                HttpContext.Current.Application["u_apellido2"] = value;
            }
        }

        public static string u_tipo
        {
            get
            {
                return HttpContext.Current.Application["u_tipo"] as string;
            }
            set
            {
                HttpContext.Current.Application["u_tipo"] = value;
            }
        }

    }
}