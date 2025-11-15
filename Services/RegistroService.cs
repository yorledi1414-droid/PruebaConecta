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
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Apellido) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return "ERROR";
            }

            if (string.IsNullOrEmpty(dto.Rol))
                return "ERROR";

            if (_repo.EmailExiste(dto.Email))
                return "ERROR";

            try
            {
                string passwordHash = Crypto.HashPassword(dto.Password);
                int userId = _repo.CrearUsuario(dto.Nombre, dto.Apellido, dto.Email, passwordHash, dto.Rol);

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





