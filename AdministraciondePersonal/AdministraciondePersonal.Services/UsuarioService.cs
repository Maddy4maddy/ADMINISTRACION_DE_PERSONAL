using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AdministraciondePersonal.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly BitacoraService _bitacoraService;

        public UsuarioService(UsuarioRepository usuarioRepository, BitacoraService bitacoraService)
        {
            _usuarioRepository = usuarioRepository;
            _bitacoraService = bitacoraService;
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

        public List<rol> ObtenerTodosRoles()
        {
            return _usuarioRepository.ObtenerTodosRoles();
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            return _usuarioRepository.ObtenerTodosUsuarios();
        }

        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            return _usuarioRepository.ObtenerUsuarioPorId(idUsuario);
        }

        public bool ValidarContrasena(string contrasena, out string mensajeError)
        {
            if (string.IsNullOrEmpty(contrasena))
            {
                mensajeError = "La contraseña es requerida";
                return false;
            }
            if (contrasena.Length < 8)
            {
                mensajeError = "La contraseña debe tener al menos 8 caracteres";
                return false;
            }
            if (!contrasena.Any(char.IsUpper))
            {
                mensajeError = "La contraseña debe tener al menos una letra mayúscula";
                return false;
            }
            if (!contrasena.Any(char.IsLower))
            {
                mensajeError = "La contraseña debe tener al menos una letra minúscula";
                return false;
            }
            if (!contrasena.Any(char.IsDigit))
            {
                mensajeError = "La contraseña debe tener al menos un número";
                return false;
            }
            if (!contrasena.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                mensajeError = "La contraseña debe tener al menos un carácter especial";
                return false;
            }
            mensajeError = "";
            return true;
        }

        public (bool success, string mensaje) CrearUsuario(Usuario usuario, string contrasena, string usuarioActual)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                return (false, "El nombre de usuario es requerido");
            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                return (false, "El nombre completo es requerido");
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                return (false, "El correo es requerido");
            if (usuario.IdRol <= 0)
                return (false, "Debe seleccionar un rol");
            if (!ValidarContrasena(contrasena, out string error))
                return (false, error);

           
            if (_usuarioRepository.ExisteNombreUsuarioConRol(usuario.NombreUsuario, usuario.IdRol))
                return (false, "El nombre de usuario ya existe con este rol");

            if (_usuarioRepository.ExisteCorreoConRol(usuario.Correo, usuario.IdRol))
                return (false, "El correo ya está registrado con este rol");

            try
            {
                int id = _usuarioRepository.CrearUsuario(usuario, contrasena);
                var usuarioParaBitacora = new { usuario.NombreUsuario, usuario.NombreCompleto, usuario.Correo, usuario.Estado, usuario.IdRol };
                _bitacoraService.RegistrarAccion(usuarioActual, $"Creación de usuario: {JsonSerializer.Serialize(usuarioParaBitacora)}");
                return (true, "Usuario creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear usuario: {ex.Message}");
            }
        }

        public (bool success, string mensaje) ActualizarUsuario(Usuario usuario, string nuevaContrasena, string usuarioActual)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                return (false, "El nombre de usuario es requerido");
            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                return (false, "El nombre completo es requerido");
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                return (false, "El correo es requerido");
            if (usuario.IdRol <= 0)
                return (false, "Debe seleccionar un rol");
            if (!string.IsNullOrEmpty(nuevaContrasena))
            {
                if (!ValidarContrasena(nuevaContrasena, out string error))
                    return (false, error);
            }

            
            if (_usuarioRepository.ExisteNombreUsuarioConRol(usuario.NombreUsuario, usuario.IdRol, usuario.IdUsuario))
                return (false, "El nombre de usuario ya existe con este rol");

            
            if (_usuarioRepository.ExisteCorreoConRol(usuario.Correo, usuario.IdRol, usuario.IdUsuario))
                return (false, "El correo ya está registrado con este rol");

            try
            {
                _usuarioRepository.ActualizarUsuario(usuario, nuevaContrasena);
                var usuarioParaBitacora = new { usuario.NombreUsuario, usuario.NombreCompleto, usuario.Correo, usuario.Estado, usuario.IdRol };
                _bitacoraService.RegistrarAccion(usuarioActual, $"Actualización de usuario: {JsonSerializer.Serialize(usuarioParaBitacora)}");
                return (true, "Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar usuario: {ex.Message}");
            }
        }

        public (bool success, string mensaje) EliminarUsuario(int idUsuario, string usuarioActual)
        {
            try
            {
                var usuario = _usuarioRepository.ObtenerUsuarioPorId(idUsuario);
                if (usuario == null)
                    return (false, "Usuario no encontrado");
                if (_usuarioRepository.TieneRegistrosRelacionados(idUsuario))
                    return (false, "No se puede eliminar un registro con datos relacionados.");
                _usuarioRepository.EliminarUsuario(idUsuario);
                var usuarioParaBitacora = new { usuario.NombreUsuario, usuario.NombreCompleto, usuario.Correo };
                _bitacoraService.RegistrarAccion(usuarioActual, $"Eliminación de usuario: {JsonSerializer.Serialize(usuarioParaBitacora)}");
                return (true, "Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        public (bool success, string mensaje) CambiarEstadoUsuario(int idUsuario, string nuevoEstado, string usuarioActual)
        {
            try
            {
                var usuario = _usuarioRepository.ObtenerUsuarioPorId(idUsuario);
                if (usuario == null)
                    return (false, "Usuario no encontrado");
                string estadoAnterior = usuario.Estado;
                _usuarioRepository.CambiarEstadoUsuario(idUsuario, nuevoEstado);
                _bitacoraService.RegistrarAccion(usuarioActual, $"Cambio de estado de usuario {usuario.NombreUsuario}: {estadoAnterior} → {nuevoEstado}");
                return (true, $"Usuario {nuevoEstado} exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al cambiar estado: {ex.Message}");
            }
        }
    }
}