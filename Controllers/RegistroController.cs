using PruebaConecta.Dtos;
using PruebaConecta.Services;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class RegistroController : Controller
    {
        private readonly RegistroService _service;

        public RegistroController()
        {
            _service = new RegistroService();
        }

        // ==============================
        //   VISTA DE REGISTRO (GET)
        // ==============================
        [HttpGet]
        public ActionResult Index()
        {
            return View(); // Carga la vista /Views/Registro/Index.cshtml
        }

        // ==============================
        //   PROCESAR REGISTRO (POST)
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterDto dto)
        {
            var resultado = _service.RegistrarUsuario(dto);

            // ❌ Registro inválido → se queda en la vista de registro
            if (resultado != "OK")
            {
                TempData["Error"] = "Registro no válido. Verifica los datos ingresados.";
                return View(dto);
            }

            // ✔ Registro exitoso → redirige al inicio de sesión
            TempData["RegistroOk"] = "Usuario registrado correctamente.";
            return RedirectToAction("home", "Home");
        }
    }
}




