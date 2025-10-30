using PruebaConecta.Utilities;
using System;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class AccountController : Controller
    {
        // GET: RContrasena
        public ActionResult RContrasena()
        {
            return View();
        }

        // POST: RContrasena (cuando el usuario envía el formulario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RContrasena(string email)
        {
            try
            {
                string codigo = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                // 🔹 HTML del correo bonito
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
                            <!-- 🔹 LOGO -->
                            <img src='https://i.postimg.cc/dQ968RLk/Logo.png' alt='Logo' style='width: 100px; margin-bottom: 15px;' />
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

                        <!-- 🔹 PIE DE PÁGINA -->
                        <footer style='text-align: center; font-size: 12px; color: #666;'>
                            <p>No contestar</strong>.</p>
                            <p>Este correo fue enviado automáticamente por <strong>ConectaTEA</strong>.</p>
                            <p>© {DateTime.Now.Year} ConectaTEA — Todos los derechos reservados.</p>
                            <a href='https://tusitio.com' style='color: #007bff; text-decoration: none;'>www.conectatea.com</a>
                        </footer>
                    </div>
                </div>";

                var gestor = new GestorCorreo();
                gestor.EnviarCorreo(email, "Recuperación de contraseña - ConéctateA", mensajeHtml, true);

                ViewBag.Mensaje = "✅ Se ha enviado un correo con las instrucciones de recuperación.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ No se pudo enviar el correo: " + ex.Message;
                return View();
            }
        }
    }
}
