using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Security.Cryptography;
using System.Text;

namespace AdministraciondePersonal.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        private string EncriptarSHA2(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Usuario Autenticar(string nombreUsuario, string contrasena)
        {
            string contrasenaEncriptada = EncriptarSHA2(contrasena);
            return _usuarioRepository.Login(nombreUsuario, contrasenaEncriptada);
        }

        public void RegistrarIntentoFallido(string nombreUsuario)
        {
            _usuarioRepository.IncrementarIntentos(nombreUsuario);
        }

        public void ResetearIntentos(string nombreUsuario)
        {
            _usuarioRepository.ResetearIntentos(nombreUsuario);
        }

        public bool UsuarioBloqueado(string nombreUsuario)
        {
            var usuario = _usuarioRepository.ObtenerPorNombre(nombreUsuario);
            return usuario != null && (usuario.Bloqueado || usuario.Estado == "bloqueado");
        }

        public int ObtenerIntentosFallidos(string nombreUsuario)
        {
            var usuario = _usuarioRepository.ObtenerPorNombre(nombreUsuario);
            return usuario?.IntentosFallidos ?? 0;
        }
    }
}