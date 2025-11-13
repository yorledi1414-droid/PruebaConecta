using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class AdminController : Controller
    {
        // ==============================
        // 🟣 Vista principal del panel admin
        // ==============================
        public ActionResult AdminVista()
        {
            return View();
        }

        // ==============================
        // 🟣 Gestión de Roles y Permisos
        // ==============================
        public ActionResult RolesPermisos()
        {
            return View();
        }

        public ActionResult CrearRol()
        {
            return View();
        }

        public ActionResult EditarRol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditarRol(string nombreRol, string descripcion, string[] permisos)
        {
            ViewBag.Mensaje = "Cambios guardados (vista sin conexión a BD)";
            return View();
        }

        // ==============================
        // 🟣 Eliminar Rol (modal)
        // ==============================
        public ActionResult EliminarRol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmarEliminarRol()
        {
            ViewBag.Mensaje = "Rol eliminado (vista sin conexión a BD)";
            return RedirectToAction("RolesPermisos");
        }

        // ==============================
        // 🟣 Gestión de Usuarios
        // ==============================
        public ActionResult GestionUsuarios()
        {
            return View();
        }

        public ActionResult DetallesUsuario()
        {
            return View();
        }

        public ActionResult OjoLogo()
        {
            return View();
        }

        public ActionResult CambiarContrasena()
        {
            return View();
        }

        public ActionResult EliminarUsuario(int id)
        {
            ViewBag.UsuarioId = id;
            return View();
        }

        [HttpPost]
        public ActionResult EliminarUsuarioConfirmado(int id)
        {
            return RedirectToAction("GestionUsuarios");
        }

        // ==============================
        // 🟣 NUEVO: Tickets de Soporte
        // ==============================
        public ActionResult Ticket()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResponderTicket()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult ResponderTicket(string respuesta)
        {
            ViewBag.Mensaje = "Respuesta enviada correctamente (simulado)";
            return RedirectToAction("Ticket");
        }


    }
}


