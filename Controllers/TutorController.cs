using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class TutorController : Controller
    {
        // ==============================
        // 🟣 VISTA PRINCIPAL DEL TUTOR
        // ==============================
        public ActionResult TutorVista()
        {
            return View();
        }

        // ==============================
        // 🟣 REGISTRAR PACIENTE
        // ==============================
        public ActionResult RegistrarPaciente()
            {
                return View();
            }

        // ==============================
        // 🟣 MENSAJES
        // ==============================
        public ActionResult Mensajes()
        {
            return View();
        }

        // ==============================
        // 🟣 AGENDA
        // ==============================
        public ActionResult Agenda()
        {
            return View();
        }

        // ==============================
        // 🟣 SOPORTE
        // ==============================
        public ActionResult Soporte()
        {
            return View();
        }

        // ==============================
        // 🟣 NOTIFICACIONES
        // ==============================
        public ActionResult Notificaciones()
        {
            return View();
        }

        // ==============================
        // 🟣 CONFIGURACIÓN
        // ==============================
        public ActionResult Configuracion()
        {
            return View();
        }

        // ==============================
        // 🟣 MI PERFIL
        // ==============================
        public ActionResult MiPerfil()
        {
            return View();
        }
    }
}
