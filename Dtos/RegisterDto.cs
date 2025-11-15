using System.Web;

namespace PruebaConecta.Dtos
{
    public class RegisterDto
    {
        // ==========================
        // DATOS GENERALES
        // ==========================
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }


        // ==========================
        // TUTOR
        // ==========================
        public string TipoDocumento_Tutor { get; set; }
        public string NumeroDocumento_Tutor { get; set; }
        public string Direccion_Tutor { get; set; }
        public HttpPostedFileBase CedulaTutor { get; set; }


        // ==========================
        // TERAPEUTA
        // ==========================
        public string TipoDocumento_Terapeuta { get; set; }
        public string NumeroDocumento_Terapeuta { get; set; }
        public string Profesion { get; set; }
        public string Especialidad { get; set; }
        public HttpPostedFileBase CertificadoProfesion { get; set; }
    }
}



