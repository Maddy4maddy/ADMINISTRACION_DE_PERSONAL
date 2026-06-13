using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class PantallasModel : PageModel
    {
        private readonly PantallaService _service;
        private readonly BitacoraService _bitacoraService;
        public string NombreUsuario { get; set; } = "";
        public string InicialAvatar { get; set; } = "";
        public string ColorAvatar { get; set; } = "#80B0AA";

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

        public void OnGet()
        {
            ListaPantallas = _service.ObtenerPantallas();

            NombreUsuario = User.Identity?.Name ?? "Usuario";

            InicialAvatar = NombreUsuario.Substring(0, 1).ToUpper();

            ColorAvatar = "#80B0AA";
        }

        public void OnPost()
        {
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
    }
}