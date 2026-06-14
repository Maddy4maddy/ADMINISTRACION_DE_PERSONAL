using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdministraciondePersonal.Pages
{
    public class UsuariosModel : PageModel
    {
        private readonly UsuarioService _usuarioService;
        private readonly PantallaService _services;

        public UsuariosModel(UsuarioService usuarioService, PantallaService pantallaService)
        {
            _usuarioService = usuarioService;
            _services = pantallaService;
        }

        public List<Usuario> Usuarios { get; set; }
        public List<Pantalla> MenuPantallas { get; set; } = new();
        public List<Rol> RolesDisponibles { get; set; }
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }
        public string Mensaje { get; set; }
        public string Error { get; set; }
        public bool MostrarModal { get; set; }

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanioPagina { get; set; } = 10;

        [BindProperty]
        public Usuario UsuarioEditando { get; set; }

        [BindProperty]
        public string NuevaContrasena { get; set; }

        [BindProperty]
        public List<int> RolesSeleccionados { get; set; } = new List<int>();

        public IActionResult OnGet(int? id, bool nuevo = false, int pagina = 1)
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

            var rolesJson = HttpContext.Session.GetString("RolesUsuario");

            if (!string.IsNullOrEmpty(rolesJson))
            {
                var rolesIds = JsonSerializer.Deserialize<List<int>>(rolesJson);

                MenuPantallas = _services.ObtenerPantallasPorRoles(rolesIds);
            }

            PaginaActual = pagina;
            CargarDatos();

            if (nuevo)
            {
                MostrarModal = true;
                UsuarioEditando = new Usuario { Estado = "activo" };
                RolesSeleccionados = new List<int>();
            }
            else if (id.HasValue && id.Value > 0)
            {
                MostrarModal = true;
                UsuarioEditando = _usuarioService.ObtenerUsuarioPorId(id.Value);

                if (UsuarioEditando == null)
                {
                    return RedirectToPage("/Usuarios", new { pagina = PaginaActual });
                }

                RolesSeleccionados = UsuarioEditando.Roles?
                    .Select(r => r.IdRol)
                    .ToList() ?? new List<int>();
            }

            return Page();
        }

        public IActionResult OnPostGuardar(int paginaActual = 1)
        {
            try
            {
                var usuarioActual = HttpContext.Session.GetString("Usuario");

                if (UsuarioEditando == null)
                {
                    Error = "Error: Datos del usuario no recibidos";
                    MostrarModal = true;
                    PaginaActual = paginaActual;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.NombreUsuario))
                {
                    Error = "El nombre de usuario es requerido";
                    MostrarModal = true;
                    PaginaActual = paginaActual;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.NombreCompleto))
                {
                    Error = "El nombre completo es requerido";
                    MostrarModal = true;
                    PaginaActual = paginaActual;
                    CargarDatos();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(UsuarioEditando.Correo))
                {
                    Error = "El correo electrónico es requerido";
                    MostrarModal = true;
                    PaginaActual = paginaActual;
                    CargarDatos();
                    return Page();
                }

                if (RolesSeleccionados == null || RolesSeleccionados.Count == 0)
                {
                    Error = "Debe seleccionar al menos un rol";
                    MostrarModal = true;
                    PaginaActual = paginaActual;
                    CargarDatos();
                    return Page();
                }

                if (UsuarioEditando.IdUsuario == 0)
                {
                    if (string.IsNullOrWhiteSpace(NuevaContrasena))
                    {
                        Error = "La contraseña es requerida para nuevos usuarios";
                        MostrarModal = true;
                        PaginaActual = paginaActual;
                        CargarDatos();
                        return Page();
                    }

                    var result = _usuarioService.CrearUsuario(
                        UsuarioEditando,
                        NuevaContrasena,
                        RolesSeleccionados,
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
                        PaginaActual = paginaActual;
                        CargarDatos();
                        return Page();
                    }
                }
                else
                {
                    var result = _usuarioService.ActualizarUsuario(
                        UsuarioEditando,
                        NuevaContrasena,
                        RolesSeleccionados,
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
                        PaginaActual = paginaActual;
                        CargarDatos();
                        return Page();
                    }
                }

                CargarDatos();
                return RedirectToPage("/Usuarios", new { pagina = paginaActual });
            }
            catch (Exception ex)
            {
                Error = "ERROR: " + ex.Message;
                MostrarModal = true;
                PaginaActual = paginaActual;
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int id, int paginaActual = 1)
        {
            var usuarioActual = HttpContext.Session.GetString("Usuario");
            var result = _usuarioService.EliminarUsuario(id, usuarioActual);
            if (result.success)
                Mensaje = result.mensaje;
            else
                Error = result.mensaje;

            return RedirectToPage("/Usuarios", new { pagina = paginaActual });
        }

        public IActionResult OnPostCambiarEstado(int id, string nuevoEstado, int paginaActual = 1)
        {
            var usuarioActual = HttpContext.Session.GetString("Usuario");
            var result = _usuarioService.CambiarEstadoUsuario(id, nuevoEstado, usuarioActual);
            if (result.success)
                Mensaje = result.mensaje;
            else
                Error = result.mensaje;

            return RedirectToPage("/Usuarios", new { pagina = paginaActual });
        }

        public IActionResult OnPostEditar(int id, int paginaActual = 1)
        {
            return RedirectToPage("/Usuarios", new { id = id, pagina = paginaActual });
        }

        private void CargarDatos()
        {
            // Obtener todos los usuarios
            var todosLosUsuarios = _usuarioService.ObtenerTodosUsuarios();
            int totalRegistros = todosLosUsuarios.Count;

            // Calcular total de páginas
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)TamanioPagina);
            if (TotalPaginas == 0) TotalPaginas = 1;

            // Validar página actual
            if (PaginaActual < 1) PaginaActual = 1;
            if (PaginaActual > TotalPaginas) PaginaActual = TotalPaginas;

            // Aplicar paginación
            Usuarios = todosLosUsuarios
                .Skip((PaginaActual - 1) * TamanioPagina)
                .Take(TamanioPagina)
                .ToList();

            RolesDisponibles = _usuarioService.ObtenerTodosRoles();
        }
    }
}