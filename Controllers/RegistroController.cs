using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class RegistroController : Controller
    {
        // GET: /Registro
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string nombre, string apellido, string cedula, string email, bool? terapeuta, bool? tutor)
        {
            // Aquí se podrían guardar datos en BD
            TempData["RegistroOk"] = "Registro guardado (simulado)";
            return RedirectToAction("Index");
        }
    }
}
