
/*
using PruebaConecta.Repositories;
using System;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    
    public class CambiarContrasenaController : Controller
    {
        private readonly UserRepository _userRepo = new UserRepository();

        // GET: CambiarContrasena
        [HttpGet]
        
        public ActionResult CambiarContrasena()
        {
            // Si no tenemos usuario en sesión, el enlace no es válido
            if (Session["ResetUserId"] == null)
            {
                ViewBag.Error = "El enlace para cambiar la contraseña no es válido o ya fue utilizado.";
            }

            return View();
        }

        // POST: CambiarContrasena
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult CambiarContrasena(string nueva, string confirmar)
        {
            if (Session["ResetUserId"] == null)
            {
                ViewBag.Error = "El enlace para cambiar la contraseña no es válido o ya fue utilizado.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(nueva) || string.IsNullOrWhiteSpace(confirmar))
            {
                ViewBag.Error = "Debes ingresar la nueva contraseña en ambos campos.";
                return View();
            }

            if (nueva != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            try
            {
                int userId = (int)Session["ResetUserId"];

                // 🔐 Actualizamos la contraseña en la BD
                _userRepo.ActualizarPassword(userId, nueva);

                // Evitar reusar el enlace
                Session["ResetUserId"] = null;

                TempData["RegistroOk"] = "Tu contraseña se actualizó correctamente. Ahora puedes iniciar sesión con la nueva contraseña.";
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                // Para el usuario mostramos algo genérico
                ViewBag.Error = "Ocurrió un error al intentar actualizar la contraseña.";
                // Si quieres, puedes loguear ex.Message internamente
                return View();
            }
        }
    }
}
*/