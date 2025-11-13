using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PruebaConecta.Controllers
{
    public class TerapeutaController : Controller
    {
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
        // 🟣 MI PERFIL
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
    }
}