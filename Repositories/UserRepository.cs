using PruebaConecta.Dtos;
using PruebaConecta.Repositories.Model;
using PruebaConecta.Services;
using System;
using System.Linq;
using System.Web.Helpers;

namespace PruebaConecta.Repositories
{
    public class UserRepository
    {
        private readonly TherapyDBEntities _db;

        public UserRepository()
        {
            _db = new TherapyDBEntities();
        }

        public bool EmailExiste(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        private int ObtenerRolId(string rolNombre)
        {
            var rol = _db.Roles.FirstOrDefault(r => r.Role_Name == rolNombre);

            if (rol == null)
                throw new Exception("El rol '" + rolNombre + "' no existe.");

            return rol.Role_ID;
        }

        private int ObtenerStateId(string stateDescription)
        {
            var estado = _db.States.FirstOrDefault(s => s.Description == stateDescription);

            if (estado == null)
            {
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
            int stateId = ObtenerStateId("Activo");

            var nuevo = new User
            {
                Name = nombre,
                LastName = apellido,
                Email = email,
                Password = Crypto.HashPassword(password),
                Registration_Date = DateTime.Now,
                Role_ID = rolId,
                State_ID = stateId
            };

            _db.Users.Add(nuevo);
            _db.SaveChanges();

            return nuevo.User_ID;
        }

        public void CrearTerapeuta(int userId, RegisterDto dto)
        {
            var terapeuta = new Therapist
            {
                User_ID = userId,
                Document_Type = dto.TipoDocumento_Terapeuta,
                Document = EncryptionService.Encrypt(dto.NumeroDocumento_Terapeuta),
                Professional_Certificate = dto.Profesion,
                Specialty = dto.Especialidad,
                Verification_Status = "Pendiente"
            };

            _db.Therapists.Add(terapeuta);
            _db.SaveChanges();
        }

        public void CrearTutor(int userId, RegisterDto dto)
        {
            var tutor = new Tutor
            {
                User_ID = userId,
                Document_Type = dto.TipoDocumento_Tutor,
                Document = EncryptionService.Encrypt(dto.NumeroDocumento_Tutor),
                Address = dto.Direccion_Tutor
            };

            _db.Tutors.Add(tutor);
            _db.SaveChanges();
        }

        // ====================== VALIDAR LOGIN (ARREGLADO) ======================
        public User ValidarUsuario(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            string stored = user.Password ?? "";

            // ✔ TU HASH ES EL NATIVO DE CRYPTO.HASHPASSWORD (BASE64)
            // NO EMPIEZA POR $2a$ NI POR $2b$ → POR ESO FALLABA TODO
            bool ok = false;

            try
            {
                ok = Crypto.VerifyHashedPassword(stored, password);
            }
            catch
            {
                ok = false;
            }

            if (ok)
                return user;

            // ✔ SOPORTE A CONTRASEÑAS EN TEXTO PLANO (por si queda alguna vieja)
            if (stored == password)
            {
                user.Password = Crypto.HashPassword(password);
                _db.SaveChanges();
                return user;
            }

            return null;
        }

        public User ObtenerUsuarioPorEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public void ActualizarPassword(int userId, string nuevaPasswordPlano)
        {
            var user = _db.Users.FirstOrDefault(u => u.User_ID == userId);

            if (user == null)
                throw new Exception("Usuario no encontrado para actualización de contraseña.");

            user.Password = Crypto.HashPassword(nuevaPasswordPlano);
            _db.SaveChanges();
        }
    }
}






