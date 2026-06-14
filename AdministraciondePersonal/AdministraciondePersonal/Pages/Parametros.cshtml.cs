using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;
using System.Text.Json;

namespace AdministraciondePersonal.Pages
{
    public class ParametrosModel : PageModel
    {
        private readonly ParametroService _service;
        private readonly PantallaService _services;
        private readonly BitacoraService _bitacoraService;

        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        public int TotalPaginas { get; set; }

        private const int RegistrosPorPagina = 10;

        public ParametrosModel(
            ParametroService service,
            BitacoraService bitacoraService,
            PantallaService pantallaService)
        {
            _service = service;
            _services = pantallaService;
            _bitacoraService = bitacoraService;
        }

        public List<Parametros> ListaParametros { get; set; } = new();

        public List<Pantalla> MenuPantallas { get; set; } = new();

        public string NombreUsuario { get; set; }

        public string InicialAvatar { get; set; }

        public string ColorAvatar { get; set; }

        public bool MostrarMensajeModal { get; set; }

        public bool EsError { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        [BindProperty]
        public int IdParametro { get; set; }

        [BindProperty]
        public string Codigo { get; set; }

        [BindProperty]
        public string Valor { get; set; }

        public IActionResult OnGet()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            var todos = _service.ObtenerParametros();

            TotalPaginas = (int)Math.Ceiling(
                (double)todos.Count / RegistrosPorPagina);

            ListaParametros = todos
                .Skip((Pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            return Page();
        }

        public void OnPost()
        {
            PrepararSesion();

            var resultado = _service.GuardarParametro(
                IdParametro,
                Codigo,
                Valor);

            var todos = _service.ObtenerParametros();

            TotalPaginas = (int)Math.Ceiling(
                (double)todos.Count / RegistrosPorPagina);

            ListaParametros = todos
                .Skip((Pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            MostrarMensajeModal = true;

            EsError = !resultado.success;

            Mensaje = resultado.mensaje;

            if (resultado.success)
            {
                if (IdParametro == 0)
                {
                    _bitacoraService.RegistrarAccion(
                        User.Identity?.Name ?? "Sistema",
                        $"Creó el parámetro: {Codigo}"
                    );
                }
                else
                {
                    _bitacoraService.RegistrarAccion(
                        User.Identity?.Name ?? "Sistema",
                        $"Modificó el parámetro ID {IdParametro} - {Codigo}"
                    );
                }
            }
        }

        public void OnPostDelete(int id)
        {
            PrepararSesion();

            var resultado = _service.EliminarParametro(id);

            var todos = _service.ObtenerParametros();

            TotalPaginas = (int)Math.Ceiling(
                (double)todos.Count / RegistrosPorPagina);

            ListaParametros = todos
                .Skip((Pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            MostrarMensajeModal = true;

            EsError = !resultado.success;

            Mensaje = resultado.mensaje;

            if (resultado.success)
            {
                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Eliminó el parámetro ID {id}"
                );
            }
        }

        public JsonResult OnGetParametro(int id)
        {
            var parametro = _service.ObtenerPorId(id);

            return new JsonResult(parametro);
        }

        private bool PrepararSesion()
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return false;
            }

            NombreUsuario = usuario;
            InicialAvatar = NombreUsuario.Substring(0, 1).ToUpper();

            int hash = 0;

            foreach (char c in NombreUsuario)
            {
                hash = c + ((hash << 5) - hash);
            }

            var colores = new[]
            {
                "#273a77", "#80B0AA", "#FDB3CA", "#315855",
                "#4A90E2", "#E74C3C", "#2ECC71", "#F39C12",
                "#9B59B6", "#1ABC9C", "#E67E22", "#3498DB"
            };

            ColorAvatar = colores[Math.Abs(hash) % colores.Length];

            var rolesJson =
                HttpContext.Session.GetString("RolesUsuario");

            if (!string.IsNullOrEmpty(rolesJson))
            {
                var rolesIds =
                    JsonSerializer.Deserialize<List<int>>(rolesJson);

                MenuPantallas =
                    _services.ObtenerPantallasPorRoles(rolesIds);
            }

            return true;
        }
    }
}