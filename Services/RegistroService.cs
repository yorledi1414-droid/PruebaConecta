using PruebaConecta.Dtos;
using PruebaConecta.Repositories;
using System;
using System.Web.Helpers;

namespace PruebaConecta.Services
{
    public class RegistroService
    {
        private readonly UserRepository _repo;

        public RegistroService()
        {
            _repo = new UserRepository();
        }

        public string RegistrarUsuario(RegisterDto dto)
        {
            // ===============================
            // VALIDACIONES BÁSICAS
            // ===============================
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Apellido) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return "ERROR";
            }

            if (string.IsNullOrEmpty(dto.Rol))
                return "ERROR";

            // 🔥 No permitir registro de Administrador desde vista
            if (dto.Rol == "Administrador")
                return "ERROR";

            if (_repo.EmailExiste(dto.Email))
                return "ERROR";

            try
            {
                // ======================================================
                // ✔ NO HASH AQUÍ — el repositorio lo hace automáticamente
                // ======================================================

                int userId = _repo.CrearUsuario(
                    dto.Nombre,
                    dto.Apellido,
                    dto.Email,
                    dto.Password,   // ← contraseña en texto plano (el repo la hashea)
                    dto.Rol
                );

                // ===============================
                // CREAR PERFIL SEGÚN EL ROL
                // ===============================
                if (dto.Rol == "Terapeuta")
                    _repo.CrearTerapeuta(userId, dto);

                if (dto.Rol == "Tutor")
                    _repo.CrearTutor(userId, dto);

                return "OK";
            }
            catch
            {
                return "ERROR";
            }
        }
    }
}







