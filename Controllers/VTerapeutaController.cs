using System;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PruebaConecta.Controllers
{
    // Clase para pie/encabezado con número de página
    public class PdfPageEvents : PdfPageEventHelper
    {
        // Fuente para el pie
        private Font footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, BaseColor.GRAY);
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Pie centrado con número de página
            PdfPTable tbl = new PdfPTable(1);
            tbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            PdfPCell cell = new PdfPCell(new Phrase($"Página {writer.PageNumber}", footerFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            tbl.AddCell(cell);
            tbl.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 5, writer.DirectContent);
        }
    }

    public class VTerapeutaController : Controller
    {
        // GET: VTerapeuta
        public ActionResult VTerapeuta()
        {
            return View();
        }

        // Generar PDF con secciones, logo, tabla y pie
        public ActionResult GenerarReporte(string paciente)
        {
            // --- 1) Datos editables manualmente ---
            string nombrePaciente = string.IsNullOrEmpty(paciente) ? "Juan Perez" : paciente;
            string edad = "14 años";
            string fechaCita = "15/09/2025";
            string horaCita = "11:34 a.m.";
            string duracion = "40 min";
            string terapeuta = "Terapeuta: Ana Herrera";
            string observaciones = "El paciente mostró avances en el juego de memoria y en el de rompe cabezas. Se le recomienda continuar los juegos de 15 a 30 minutos por día.";
            
            // ------------------------------------------------

            // --- 2) Preparar stream y documento ---
            using (MemoryStream ms = new MemoryStream())
            {
                // Opciones de página: A4 y márgenes
                Document doc = new Document(PageSize.A4, 40f, 40f, 60f, 50f);

                // Vincular writer al documento y al MemoryStream
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                // --- 3) Registrar eventos (pie de página con números) ---
                writer.PageEvent = new PdfPageEvents();

                // --- 4) Abrir documento para comenzar a escribir ---
                doc.Open();

                // 4.a) Metadatos (opcional)
                doc.AddAuthor("ConectaTEA");
                doc.AddCreator("Reporte ConectaTea");
                doc.AddTitle($"Reporte_{nombrePaciente}");

                // 4.b) Agregar logo (si existe)
                string logoPath = Server.MapPath("~/Content/Images/logo.png"); // coloca tu logo ahí
                if (System.IO.File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(80f, 80f);
                    logo.Alignment = Image.ALIGN_RIGHT; // a la derecha del encabezado
                    doc.Add(logo);
                }

                // 4.c) Fuentes (puedes usar TTF si quieres incrustar)
                // Ejemplo con fuente TTF (opcional):
                // string fontPath = Server.MapPath("~/Content/Fonts/Roboto-Regular.ttf");
                // BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                // Font normal = new Font(bf, 11);

                Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                Font subtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.DARK_GRAY);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
                Font smallGray = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, BaseColor.GRAY);

                // 4.d) Encabezado textual
                Paragraph pTitle = new Paragraph("REPORTE DE AVANCES", titulo);
                pTitle.Alignment = Element.ALIGN_LEFT;
                doc.Add(pTitle);

                // Separador pequeño
                doc.Add(new Paragraph("\n"));

                // 4.e) Información meta (quién generó, fecha/hora)
                Paragraph meta = new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}    Reporte generado por terapeuta Ana Herrera {User?.Identity?.Name ?? "Demo"}", smallGray);
                doc.Add(meta);

                doc.Add(new Paragraph("\n"));

                // 4.f) Bloque con datos del paciente: usaremos una tabla para alinear
                PdfPTable patientTable = new PdfPTable(new float[] { 1f, 2f });
                patientTable.WidthPercentage = 100;
                patientTable.SpacingAfter = 10f;

                // Celda de etiqueta (avatar o título)
                PdfPCell c1 = new PdfPCell(new Phrase("Paciente", subtitulo));
                c1.Border = Rectangle.NO_BORDER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                c1.PaddingBottom = 5;
                patientTable.AddCell(c1);

                // Celda de datos
                PdfPCell c2 = new PdfPCell();
                c2.Border = Rectangle.NO_BORDER;
                c2.AddElement(new Phrase($"Nombre: {nombrePaciente}", normalFont));
                c2.AddElement(new Phrase($"Edad: {edad}", normalFont));
                c2.AddElement(new Phrase($"Fecha cita: {fechaCita}  {horaCita}", normalFont));
                c2.AddElement(new Phrase($"Duración: {duracion}", normalFont));
                doc.Add(new Paragraph(terapeuta, normalFont));
                patientTable.AddCell(c2);

                doc.Add(patientTable);

                // 4.g) Observaciones (título + párrafo)
                doc.Add(new Paragraph("Observaciones:", subtitulo));
                Paragraph obsPara = new Paragraph(observaciones, normalFont);
                obsPara.SpacingAfter = 10f;
                doc.Add(obsPara);

                // 4.h) Tabla de Avances por Juego
                doc.Add(new Paragraph("Avances por Juego:", subtitulo));
                PdfPTable t = new PdfPTable(new float[] { 3f, 1f, 3f });
                t.WidthPercentage = 100;
                t.SpacingBefore = 6f;
                t.SpacingAfter = 10f;

                // Encabezados con fondo claro
                PdfPCell h1 = new PdfPCell(new Phrase("Juego", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));
                h1.BackgroundColor = new BaseColor(230, 230, 250);
                h1.HorizontalAlignment = Element.ALIGN_CENTER;
                h1.Padding = 6;
                t.AddCell(h1);

                PdfPCell h2 = new PdfPCell(new Phrase("Puntaje", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));
                h2.BackgroundColor = new BaseColor(230, 230, 250);
                h2.HorizontalAlignment = Element.ALIGN_CENTER;
                h2.Padding = 6;
                t.AddCell(h2);

                PdfPCell h3 = new PdfPCell(new Phrase("Comentario", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)));
                h3.BackgroundColor = new BaseColor(230, 230, 250);
                h3.HorizontalAlignment = Element.ALIGN_CENTER;
                h3.Padding = 6;
                t.AddCell(h3);

                // Filas de ejemplo (reemplaza por tus datos)
                t.AddCell(new PdfPCell(new Phrase("Juego de Memoria", normalFont)) { Padding = 6 });
                t.AddCell(new PdfPCell(new Phrase("85", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 6 });
                t.AddCell(new PdfPCell(new Phrase("Mejoró reconocimiento de patrones y memorizando con mas facilidad la ubicacion de parejas", normalFont)) { Padding = 6 });

                t.AddCell(new PdfPCell(new Phrase("Rompe Cabezas", normalFont)) { Padding = 6 });
                t.AddCell(new PdfPCell(new Phrase("78", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 6 });
                t.AddCell(new PdfPCell(new Phrase("Logro armar el rompe cabezas con mas rapidez que en otras ocasiones", normalFont)) { Padding = 6 });

                doc.Add(t);

                // 4.i) Salto de página si quieres más contenido en otra página
                // doc.NewPage(); // usa esto para forzar un salto de página

                // 4.j) Pie adicional (texto centrado) antes del cierre
                Paragraph foot = new Paragraph("ConectaTEA - conectaTEA@gmail.com - Tel: 12345678", smallGray);
                foot.Alignment = Element.ALIGN_CENTER;
                doc.Add(foot);

                // --- 5) Cerrar documento y obtener bytes ---
                doc.Close();      // importante: cierra el PDF y libera recursos
                byte[] fileBytes = ms.ToArray();

                // --- 6) Devolver el FileResult para descarga ---
                string fileName = $"Reporte_{nombrePaciente.Replace(" ", "_")}.pdf";
                return File(fileBytes, "application/pdf", fileName);
            } // using MemoryStream -> se cierra automáticamente
        }
    }
}
