using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class UsuariosModel : PageModel
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosModel(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public List<Usuario> Usuarios { get; set; }
        public List<rol> RolesDisponibles { get; set; }
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }
        public string Mensaje { get; set; }
        public string Error { get; set; }
        public bool MostrarModal { get; set; }

        [BindProperty]
        public Usuario UsuarioEditando { get; set; }
        [BindProperty]
        public string NuevaContrasena { get; set; }

        public IActionResult OnGet(int? id, bool nuevo = false)
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            NombreUsuario = usuario;
            InicialAvatar = NombreUsuario?.Substring(0, 1).ToUpper();

            int hash = 0;
            foreach (char c in NombreUsuario ?? "U")
                hash = c + ((hash << 5) - hash);
            var colores = new[] { "#273a77", "#80B0AA", "#FDB3CA", "#315855", "#4A90E2", "#E74C3C" };
            ColorAvatar = colores[Math.Abs(hash) % colores.Length];

            CargarDatos();

            if (nuevo)
            {
                MostrarModal = true;
                UsuarioEditando = new Usuario { Estado = "activo", IdRol = 0 };
            }
            else if (id.HasValue && id.Value > 0)
            {
                MostrarModal = true;
                UsuarioEditando = _usuarioService.ObtenerUsuarioPorId(id.Value);
                if (UsuarioEditando == null)
                {
                    return RedirectToPage("/Usuarios");
                }
            }

            return Page();
        }

        public IActionResult OnPostGuardar()
        {
            try
            {
                var usuarioActual = HttpContext.Session.GetString("Usuario");

                if (UsuarioEditando == null)
                {
                    Error = "Error: Datos del usuario no recibidos";
                    MostrarModal = true;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.NombreUsuario))
                {
                    Error = "El nombre de usuario es requerido";
                    MostrarModal = true;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.NombreCompleto))
                {
                    Error = "El nombre completo es requerido";
                    MostrarModal = true;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.Correo))
                {
                    Error = "El correo electrónico es requerido";
                    MostrarModal = true;
                    CargarDatos();
                    return Page();
                }

                if (UsuarioEditando.IdRol <= 0)
                {
                    Error = "Debe seleccionar un rol";
                    MostrarModal = true;
                    CargarDatos();
                    return Page();
                }

                if (UsuarioEditando.IdUsuario == 0)
                {
                    if (string.IsNullOrWhiteSpace(NuevaContrasena))
                    {
                        Error = "La contraseña es requerida para nuevos usuarios";
                        MostrarModal = true;
                        CargarDatos();
                        return Page();
                    }

                    var result = _usuarioService.CrearUsuario(
                        UsuarioEditando,
                        NuevaContrasena,
                        usuarioActual
                    );

                    if (result.success)
                    {
                        Mensaje = result.mensaje;
                        MostrarModal = false;
                    }
                    else
                    {
                        Error = result.mensaje;
                        MostrarModal = true;
                        CargarDatos();
                        return Page();
                    }
                }
                else
                {
                    var result = _usuarioService.ActualizarUsuario(
                        UsuarioEditando,
                        NuevaContrasena,
                        usuarioActual
                    );

                    if (result.success)
                    {
                        Mensaje = result.mensaje;
                        MostrarModal = false;
                    }
                    else
                    {
                        Error = result.mensaje;
                        MostrarModal = true;
                        CargarDatos();
                        return Page();
                    }
                }

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                Error = "ERROR: " + ex.ToString();
                MostrarModal = true;
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            var usuarioActual = HttpContext.Session.GetString("Usuario");
            var result = _usuarioService.EliminarUsuario(id, usuarioActual);
            if (result.success)
                Mensaje = result.mensaje;
            else
                Error = result.mensaje;

            CargarDatos();
            return Page();
        }

        public IActionResult OnPostCambiarEstado(int id, string nuevoEstado)
        {
            var usuarioActual = HttpContext.Session.GetString("Usuario");
            var result = _usuarioService.CambiarEstadoUsuario(id, nuevoEstado, usuarioActual);
            if (result.success)
                Mensaje = result.mensaje;
            else
                Error = result.mensaje;

            CargarDatos();
            return Page();
        }

        public IActionResult OnPostEditar(int id)
        {
            return RedirectToPage("/Usuarios", new { id = id });
        }

        private void CargarDatos()
        {
            Usuarios = _usuarioService.ObtenerTodosUsuarios();
            RolesDisponibles = _usuarioService.ObtenerTodosRoles();
        }
    }
}