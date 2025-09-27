using System.Web.Mvc;

namespace TuProyecto.Controllers
{
    public class JuegosController : Controller
    {
        // Página principal de Juegos
        public ActionResult Index()
        {
            return View();
        }

        // Juego: Rompecabezas
        public ActionResult Rompecabezas()
        {
            return View();
        }

        // Juego: Memoria
        public ActionResult Memoria()
        {
            return View();
        }
    }
}
