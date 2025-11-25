using PruebaConecta.Repositories;      // ← AGREGADO
using PruebaConecta.Repositories.Model;
using PruebaConecta.Utilities;
using System;
using System.Linq;
using System.Web.Helpers;              // ← Para Crypto.HashPassword
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class AccountController : Controller
    {
        // ================================
        // 1. MOSTRAR RECUPERAR CONTRASEÑA
        // ================================
        public ActionResult RContrasena()
        {
            return View();
        }

        // ================================
        // 2. ENVIAR CÓDIGO POR CORREO
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RContrasena(string email)
        {
            try
            {
                string codigo = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                string mensajeHtml = $@"
                <div style='
                    font-family: Arial, sans-serif;
                    background: linear-gradient(135deg, #007bff, #00c6ff);
                    padding: 30px;
                    border-radius: 10px;
                    color: #ffffff;
                    max-width: 600px;
                    margin: 20px auto;
                '>
                    <div style='background: #ffffff; border-radius: 10px; color: #333; padding: 30px;'>
                        <div style='text-align: center;'>
                            <img src='https://i.postimg.cc/VNq6JVzx/Logo.png' alt='Logo' style='width: 100px; margin-bottom: 15px;' />
                            <h2 style='color: #007bff;'>Recuperación de Contraseña</h2>
                        </div>

                        <p>Hola,</p>
                        <p>Has solicitado restablecer tu contraseña. Usa el siguiente código para continuar con el proceso:</p>

                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='
                                font-size: 26px;
                                font-weight: bold;
                                color: #007bff;
                                background: #eaf3ff;
                                padding: 12px 25px;
                                border-radius: 6px;
                                display: inline-block;
                            '>{codigo}</span>
                        </div>

                        <p>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>

                        <hr style='border: none; border-top: 1px solid #ddd; margin: 25px 0;' />

                        <footer style='text-align: center; font-size: 12px; color: #666;'>
                            <p>No contestar.</p>
                            <p>Este correo fue enviado automáticamente por <strong>ConectaTEA</strong>.</p>
                            <p>© {DateTime.Now.Year} ConectaTEA — Todos los derechos reservados.</p>
                        </footer>
                    </div>
                </div>";

                var gestor = new GestorCorreo();
                gestor.EnviarCorreo(email, "Recuperación de contraseña - ConéctateA", mensajeHtml, true);

                Session["ResetEmail"] = email;
                Session["ResetCode"] = codigo;

                return RedirectToAction("VerificarContrasena");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ No se pudo enviar el correo: " + ex.Message;
                return View();
            }
        }

        // ================================
        // 3. MOSTRAR VERIFICAR CÓDIGO
        // ================================
        public ActionResult VerificarContrasena()
        {
            var email = Session["ResetEmail"] as string;

            if (email == null)
                return RedirectToAction("RContrasena");

            ViewBag.Email = email;
            return View("~/Views/Home/VerificarContrasena.cshtml");
        }

        // ================================
        // 4. VALIDAR CÓDIGO INGRESADO
        // ================================
        [HttpPost]
        public ActionResult VerificarContrasena(string codigo)
        {
            string codigoCorrecto = Session["ResetCode"] as string;
            string email = Session["ResetEmail"] as string;

            if (email == null || codigoCorrecto == null)
                return RedirectToAction("RContrasena");

            if (codigo != null && codigo.Trim().ToUpper() == codigoCorrecto.Trim().ToUpper())
            {
                // Buscar usuario por email
                using (var db = new TherapyDBEntities())
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        // Guardar UserID en sesión
                        Session["ResetUserId"] = user.User_ID;
                    }
                }

                return RedirectToAction("CambiarContrasena");
            }

            ViewBag.Email = email;
            ViewBag.Error = "El código ingresado es incorrecto.";
            return View("~/Views/Home/VerificarContrasena.cshtml");
        }


        // ================================
        // 5. MOSTRAR CAMBIAR CONTRASEÑA
        // ================================
        public ActionResult CambiarContrasena()
        {
            if (Session["ResetEmail"] == null)
                return RedirectToAction("RContrasena");

            return View("~/Views/CambiarContrasena/CambiarContrasena.cshtml");
        }

        // ================================
        // 6. GUARDAR CONTRASEÑA EN BD  ✅ 🔥
        // ================================
        [HttpPost]
        public ActionResult CambiarContrasena(string nueva, string confirmar)
        {
            if (Session["ResetEmail"] == null || Session["ResetUserId"] == null)
                return RedirectToAction("RContrasena");

            if (nueva != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View("~/Views/CambiarContrasena/CambiarContrasena.cshtml");
            }

            int userId = (int)Session["ResetUserId"];

            try
            {
                var repo = new UserRepository();

                // 🚀 ACTUALIZAR DIRECTO — sin usar un contexto extra
                repo.ActualizarPassword(userId, nueva);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ No se pudo actualizar la contraseña: " + ex.Message;
                return View("~/Views/CambiarContrasena/CambiarContrasena.cshtml");
            }

            // ✔ limpiar sesiones
            Session.Remove("ResetEmail");
            Session.Remove("ResetCode");
            Session.Remove("ResetUserId");

            TempData["RegistroOk"] = "Tu contraseña se actualizó correctamente. Ahora puedes iniciar sesión con la nueva contraseña.";

            return RedirectToAction("Home", "Home");
        }


    }
}



