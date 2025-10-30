using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PruebaConecta.Utilities
{
    public class GestorCorreo
    {
        private readonly SmtpClient cliente;
        private readonly string Host = "smtp.gmail.com";
        private readonly int Port = 587;
        private readonly string User = "conectatea.soporte@gmail.com";
        private readonly string Password = "bjjlqvlietayhplh";
        private readonly bool EnableSSL = true;

         public GestorCorreo()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }

        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtml = false)
        {
            destinatario = destinatario?.Trim();

            if (string.IsNullOrEmpty(destinatario))
                throw new Exception("La dirección de correo del destinatario está vacía.");

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // ✅ fuerza TLS

            using (var email = new MailMessage(User, destinatario, asunto, mensaje))
            {
                email.IsBodyHtml = esHtml;
                cliente.Send(email);
            }
        }
    }
}