using PruebaConecta.Dtos;
using PruebaConecta.Services;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _service;

        public HomeController()
        {
            _service = new HomeService();
        }

        // ==============================
        //  PANTALLA PRINCIPAL (LOGIN)
        // ==============================
        [HttpGet]
        public ActionResult Home()
        {
            // Carga la vista /Views/Home/home.cshtml
            return View("home");
        }

        // ==============================
        //  PROCESAR LOGIN
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Home(HomeDto dto)
        {
            var user = _service.IniciarSesion(dto);

            if (user == null)
            {
                TempData["Error"] = "Correo o contraseña incorrectos.";
                return RedirectToAction("Home", "Home");
            }

            // Guardar sesión
            Session["UserID"] = user.User_ID;
            Session["UserName"] = user.Name;
            Session["UserRole"] = user.Role.Role_Name;

            // Redirigir según el rol
            switch (user.Role.Role_Name)
            {
                case "Administrador":
                    return RedirectToAction("AdminVista", "Admin");
                case "Terapeuta":
                    return RedirectToAction("TerapeutaVista", "Terapeuta");
                case "Tutor":
                    return RedirectToAction("TutorVista", "Tutor");
                default:
                    TempData["Error"] = "Rol no reconocido.";
                    return RedirectToAction("Home", "Home");
            }
        }

        // ==============================
        //  CERRAR SESIÓN
        // ==============================
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Home", "Home");
        }

        // ==============================
        //  PÁGINAS ADICIONALES
        // ==============================
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult VerificarContrasena()
        {
            return View();
        }
    }
}
