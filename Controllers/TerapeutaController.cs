using iTextSharp.text;
using iTextSharp.text.pdf;
using PruebaConecta.Repositories.Model;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class TerapeutaController : Controller
    {
        private readonly TherapyDBEntities _db;

        public TerapeutaController()
        {
            _db = new TherapyDBEntities();
        }

        // GET: Terapeuta
        public ActionResult TerapeutaVista()
        {
            return View();
        }

        // ==============================
        // 🟣 CONFIGURACIÓN
        // ==============================
        public ActionResult ConfiguracionTerapeuta()
        {
            return View();
        }

        // ==============================
        // 🟣 MI PERFIL
        // ==============================
        public ActionResult MiPerfilTerapeuta()
        {
            return View();
        }

        // ==============================
        // 🟣 MIS PACIENTES
        // ==============================
        public ActionResult MisPacientes()
        {
            return View();
        }

        // ==============================
        // 🟣 REGISTRAR PROGRESO
        // ==============================
        public ActionResult RegistrarProgreso()
        {
            return View();
        }

        // ==============================
        // 🟣 AGENDA DE CITAS
        // ==============================
        public ActionResult AgendaDeCitas()
        {
            return View();
        }

        // ==============================
        // 🟣 NUEVA CITA
        // ==============================
        public ActionResult NuevaCita()
        {
            return View();
        }

        // ==============================
        // 🟣 REPORTES
        // ==============================
        public ActionResult Reportes()
        {
            return View();
        }

        // ==============================
        // 🟣 NOTIFICACIONES
        // ==============================
        public ActionResult NotificacionesTerapeuta()
        {
            return View();
        }

        // ==============================
        // 🟣 MENSAJES
        // ==============================
        public ActionResult MensajesTerapeuta()
        {
            return View();
        }

        // ===========================================================
        // 📄 GENERAR PDF DEL REPORTE DEL TERAPEUTA (DATOS REALES)
        // ===========================================================
        public FileResult GenerarPDF()
        {
            // Validar sesión
            if (Session["UserID"] == null)
                throw new Exception("Debe iniciar sesión para generar el reporte.");

            int userId = Convert.ToInt32(Session["UserID"]);

            // Buscar terapeuta en la base de datos
            var terapeuta = _db.Therapists.FirstOrDefault(t => t.User_ID == userId);
            if (terapeuta == null)
                throw new Exception("No se encontró el terapeuta en la base de datos.");

            // Ejemplo de datos: tutores/pacientes relacionados (ajusta según tus tablas)
            var pacientes = _db.Tutors
                .Select(t => new
                {
                    Nombre = t.User.Name + " " + t.User.LastName,
                    Documento = t.Document,
                    Tipo = t.Document_Type,
                    Direccion = t.Address
                })
                .ToList();

            // Crear documento PDF
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40f, 40f, 60f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // =========================
                // LOGO Y ENCABEZADO
                // =========================
                string logoPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleAbsolute(70, 70);
                    logo.Alignment = Element.ALIGN_LEFT;
                    doc.Add(logo);
                }

                Paragraph titulo = new Paragraph("REPORTE GENERAL DEL TERAPEUTA", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLUE));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC, BaseColor.DARK_GRAY)));
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Terapeuta: {terapeuta.User.Name} {terapeuta.User.LastName}", new Font(Font.FontFamily.HELVETICA, 12)));
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Pacientes Asignados:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                doc.Add(new Paragraph(" "));

                // =========================
                // TABLA DE PACIENTES
                // =========================
                PdfPTable tabla = new PdfPTable(4);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 3f, 2f, 2f, 3f });

                // Encabezados
                string[] columnas = { "Nombre del Tutor", "Documento", "Tipo", "Dirección" };
                foreach (string c in columnas)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(c, new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
                    celda.BackgroundColor = new BaseColor(100, 149, 237);
                    celda.HorizontalAlignment = Element.ALIGN_CENTER;
                    celda.Padding = 6;
                    tabla.AddCell(celda);
                }

                // Datos
                foreach (var p in pacientes)
                {
                    tabla.AddCell(p.Nombre);
                    tabla.AddCell(p.Documento);
                    tabla.AddCell(p.Tipo);
                    tabla.AddCell(p.Direccion);
                }

                doc.Add(tabla);
                doc.Add(new Paragraph(" "));

                // =========================
                // PIE DE PÁGINA
                // =========================
                Paragraph pie = new Paragraph("Reporte generado automáticamente por ConectaTEA © " + DateTime.Now.Year,
                    new Font(Font.FontFamily.HELVETICA, 9, Font.ITALIC, BaseColor.GRAY));
                pie.Alignment = Element.ALIGN_CENTER;
                doc.Add(pie);

                doc.Close();
                writer.Close();

                return File(ms.ToArray(), "application/pdf", "Reporte_Terapeuta.pdf");
            }
        }
    }
}
