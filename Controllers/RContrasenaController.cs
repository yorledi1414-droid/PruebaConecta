using PruebaConecta.Utilities;
using System;
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
                // 🔹 Generar código de verificación
                string codigo = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                // 🔹 HTML del correo (TU MISMO HTML ORIGINAL)
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
                            <img src='https://postimg.cc/sQyPsQzf' alt='Logo' style='width: 100px; margin-bottom: 15px;' />
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

                        <p>Si no solicitaste este cambio, puedes ignorar este mensaje. Tu cuenta seguirá segura.</p>

                        <hr style='border: none; border-top: 1px solid #ddd; margin: 25px 0;' />

                        <footer style='text-align: center; font-size: 12px; color: #666;'>
                            <p>No contestar</strong>.</p>
                            <p>Este correo fue enviado automáticamente por <strong>ConectaTEA</strong>.</p>
                            <p>© {DateTime.Now.Year} ConectaTEA — Todos los derechos reservados.</p>
                        </footer>
                    </div>
                </div>";

                // 🔹 Enviar correo (NO SE MODIFICA)
                var gestor = new GestorCorreo();
                gestor.EnviarCorreo(email, "Recuperación de contraseña - ConéctateA", mensajeHtml, true);

                // 🔥🔥🔥 AQUI ESTABA EL PROBLEMA:
                // GUARDA EL EMAIL Y EL CÓDIGO EN SESSION
                Session["ResetEmail"] = email;
                Session["ResetCode"] = codigo;

                // 🔹 Redirigir a verificar código
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

            // 🔹 Comparación exacta del código
            if (codigo != null && codigo.Trim().ToUpper() == codigoCorrecto.Trim().ToUpper())
            {
                // Código correcto → redirigir a cambiar contraseña
                return RedirectToAction("CambiarContrasena");
            }

            // Si es incorrecto
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
        // 6. GUARDAR CONTRASEÑA EN BD
        // ================================
        [HttpPost]
        public ActionResult CambiarContrasena(string nueva, string confirmar)
        {
            if (Session["ResetEmail"] == null)
                return RedirectToAction("RContrasena");

            if (nueva != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View("~/Views/CambiarContrasena/CambiarContrasena.cshtml");
            }

            string email = Session["ResetEmail"] as string;

            // ============================================
            // 🔹 AQUI VA TU LOGICA REAL PARA ACTUALIZAR EN BD
            // ============================================

            /*
            using (var db = new TherapyDBEntities())
            {
                var user = db.Usuarios.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    user.Password = nueva; // encripta si deseas
                    db.SaveChanges();
                }
            }
            */

            // Limpiar sesiones
            Session.Remove("ResetEmail");
            Session.Remove("ResetCode");

            // Volver al login
            return RedirectToAction("Home", "Home");
        }
    }
}


