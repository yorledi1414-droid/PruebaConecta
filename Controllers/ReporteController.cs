using System;
using System.Web.Mvc;
using Rotativa;

namespace PruebaConecta.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult HistoriaClinica()
        {
            return View();
        }

        public ActionResult GenerarReporte()
        {
            var pdf = new ActionAsPdf("HistoriaClinica", new { esPDF = "true" }) 
            {
                FileName = $"Reporte_Paciente_{DateTime.Now:dd-MM-yyyy}.pdf"
            };
            return pdf;
        }
    }
}








