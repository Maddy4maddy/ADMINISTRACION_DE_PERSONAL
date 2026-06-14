using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;


//AGREGAR ESTO
using System.Text.Json;
//HASTA AQUI

namespace AdministraciondePersonal.Pages
{
    public class PantallasModel : PageModel
    {

        private readonly PantallaService _service;
        private readonly BitacoraService _bitacoraService;
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }

        //AGREGAR ESTO
        public List<Pantalla> MenuPantallas { get; set; } = new();
        //HASTA AQUI
        public PantallasModel(
            PantallaService service,
            BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        public List<Pantalla> ListaPantallas { get; set; } = new();

        [BindProperty]
        public int IdPantalla { get; set; }

        [BindProperty]
        public string NombrePantalla { get; set; }

        [BindProperty]
        public string Ruta { get; set; }

        // Propiedades para los modales
        public bool MostrarMensajeModal { get; set; }

        public bool EsError { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            ListaPantallas = _service.ObtenerPantallas();

            return Page();
        }

        public void OnPost()
        {

            PrepararSesion();

            var resultado = _service.GuardarPantalla(
                IdPantalla,
                NombrePantalla,
                Ruta);

            ListaPantallas = _service.ObtenerPantallas();

            MostrarMensajeModal = true;

            EsError = !resultado.success;

            Mensaje = resultado.mensaje;

            if (resultado.success)
            {
                if (IdPantalla == 0)
                {
                    _bitacoraService.RegistrarAccion(
                        User.Identity?.Name ?? "Sistema",
                        $"Creó la pantalla: {NombrePantalla}"
                    );
                }
                else
                {
                    _bitacoraService.RegistrarAccion(
                        User.Identity?.Name ?? "Sistema",
                        $"Modificó la pantalla ID {IdPantalla} - {NombrePantalla}"
                    );
                }
            }
        }

        public void OnPostDelete(int id)
        {
            PrepararSesion();
            var resultado = _service.EliminarPantalla(id);

            ListaPantallas = _service.ObtenerPantallas();

            MostrarMensajeModal = true;

            EsError = !resultado.success;

            Mensaje = resultado.mensaje;

            if (resultado.success)
            {
                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Eliminó la pantalla ID {id}"
                );
            }
        }

        public JsonResult OnGetPantalla(int id)
        {
            var pantalla = _service.ObtenerPorId(id);

            return new JsonResult(pantalla);
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

            //AGREGAR ESTO
            var rolesJson =
                HttpContext.Session.GetString("RolesUsuario");

            if (!string.IsNullOrEmpty(rolesJson))
            {
                var rolesIds =
                    JsonSerializer.Deserialize<List<int>>(rolesJson);

                MenuPantallas =
                    _service.ObtenerPantallasPorRoles(rolesIds);
            }

            //HASTA AQUI

            return true;
        }
    }
}