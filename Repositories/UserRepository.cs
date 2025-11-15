using PruebaConecta.Dtos;
using PruebaConecta.Repositories.Model;
using System;
using System.Linq;
using System.Web.Helpers; // ✅ Para VerifyHashedPassword

namespace PruebaConecta.Repositories
{
    public class UserRepository
    {
        private readonly TherapyDBEntities _db;

        public UserRepository()
        {
            _db = new TherapyDBEntities();
        }

        // ====================== CORREO EXISTE ======================
        public bool EmailExiste(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        // ====================== OBTENER ROLE_ID POR NOMBRE ======================
        private int ObtenerRolId(string rolNombre)
        {
            var rol = _db.Roles.FirstOrDefault(r => r.Role_Name == rolNombre);

            if (rol == null)
                throw new Exception("El rol '" + rolNombre + "' no existe en la tabla Roles.");

            return rol.Role_ID;
        }

        // ====================== OBTENER STATE_ID POR NOMBRE ======================
        private int ObtenerStateId(string stateDescription)
        {
            var estado = _db.States.FirstOrDefault(s => s.Description == stateDescription);

            if (estado == null)
            {
                // Si no existe, lo creo automáticamente
                estado = new State { Description = stateDescription };
                _db.States.Add(estado);
                _db.SaveChanges();
            }

            return estado.State_ID;
        }

        // ====================== CREAR USUARIO ======================
        public int CrearUsuario(string nombre, string apellido, string email, string password, string rolNombre)
        {
            int rolId = ObtenerRolId(rolNombre);
            int stateId = ObtenerStateId("Activo");   // estado inicial

            var nuevo = new User
            {
                Name = nombre,
                LastName = apellido,
                Email = email,
                Password = password,
                Registration_Date = DateTime.Now,
                Role_ID = rolId,
                State_ID = stateId
                // Phone, Avatar pueden ir null
            };

            _db.Users.Add(nuevo);
            _db.SaveChanges();

            return nuevo.User_ID;
        }

        // ====================== CREAR TERAPEUTA ======================
        public void CrearTerapeuta(int userId, RegisterDto dto)
        {
            var terapeuta = new Therapist
            {
                User_ID = userId,
                Document_Type = dto.TipoDocumento_Terapeuta,
                Document = dto.NumeroDocumento_Terapeuta,
                Professional_Certificate = dto.Profesion,
                Specialty = dto.Especialidad,
                Verification_Status = "Pendiente"
            };

            _db.Therapists.Add(terapeuta);
            _db.SaveChanges();
        }

        // ====================== CREAR TUTOR ======================
        public void CrearTutor(int userId, RegisterDto dto)
        {
            var tutor = new Tutor
            {
                User_ID = userId,
                Document_Type = dto.TipoDocumento_Tutor,
                Document = dto.NumeroDocumento_Tutor,
                Address = dto.Direccion_Tutor
            };

            _db.Tutors.Add(tutor);
            _db.SaveChanges();
        }

        // ====================== VALIDAR LOGIN ======================
        public User ValidarUsuario(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            bool passwordCorrecta = Crypto.VerifyHashedPassword(user.Password, password);

            return passwordCorrecta ? user : null;
        }
    }
}




