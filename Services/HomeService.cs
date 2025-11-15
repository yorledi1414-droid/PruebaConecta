using PruebaConecta.Dtos;
using PruebaConecta.Repositories;
using PruebaConecta.Repositories.Model;

namespace PruebaConecta.Services
{
    public class HomeService
    {
        private readonly UserRepository _repo;

        public HomeService()
        {
            _repo = new UserRepository();
        }

        public User IniciarSesion(HomeDto dto)
        {
            return _repo.ValidarUsuario(dto.Email, dto.Password);
        }
    }
}
