using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

using System.Text.Json;

namespace AdministraciondePersonal.Pages
{
    public class CompaniasModel : PageModel
    {
        private readonly CompaniaService _service;
        private readonly PantallaService _services;
        private readonly BitacoraService _bitacoraService;

        public CompaniasModel(
            CompaniaService service,
            PantallaService pantallaService,
            BitacoraService bitacoraService)
        {
            _service = service;
            _services = pantallaService;
            _bitacoraService = bitacoraService;
        }
        public List<Pantalla> MenuPantallas { get; set; } = new();
        [BindProperty]
        public Compania Compania { get; set; } = new();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<Compania> ListaCompanias { get; set; } = new();

        public bool MostrarFormulario { get; set; }

        public bool MostrarMensajeModal { get; set; }

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public string NombreUsuario { get; set; }

        public string InicialAvatar { get; set; }

        public string ColorAvatar { get; set; }

        private bool PrepararSesion()
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return false;
            }

            NombreUsuario = usuario;

            InicialAvatar =
                NombreUsuario.Substring(0, 1).ToUpper();

            int hash = 0;

            foreach (char c in NombreUsuario)
            {
                hash = c + ((hash << 5) - hash);
            }

            var colores = new[]
            {
                "#273a77",
                "#80B0AA",
                "#FDB3CA",
                "#315855",
                "#4A90E2",
                "#E74C3C",
                "#2ECC71",
                "#F39C12",
                "#9B59B6",
                "#1ABC9C",
                "#E67E22",
                "#3498DB"
            };

            ColorAvatar =
                colores[Math.Abs(hash) % colores.Length];

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

        public IActionResult OnGet()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login",
                    new { expirada = true });
            }

            try
            {
                ListaCompanias =
                    _service.ObtenerCompanias();

                MostrarFormulario = false;

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Consultó compañías.");

                return Page();
            }
            catch (Exception ex)
            {
                Error =
                    "Ocurrió un error al consultar las compañías.";

                MostrarMensajeModal = true;

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error consultando compañías: " + ex.Message);

                return Page();
            }
        }

        public IActionResult OnPostGuardar()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login",
                    new { expirada = true });
            }

            try
            {
                string resultado;

                if (Compania.IdCompania == 0)
                {
                    resultado =
                        _service.CrearCompania(
                            Compania.NombreCompania);

                    if (resultado == "OK")
                    {
                        Mensaje =
                            "La compañía ha sido registrada correctamente.";

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Registro de compañía '{Compania.NombreCompania}'.");
                    }
                    else
                    {
                        Error = resultado;
                    }
                }
                else
                {
                    resultado =
                        _service.EditarCompania(
                            Compania.IdCompania,
                            Compania.NombreCompania);

                    if (resultado == "OK")
                    {
                        Mensaje =
                            "La compañía ha sido actualizada correctamente.";

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de compañía '{Compania.NombreCompania}'.");
                    }
                    else
                    {
                        Error = resultado;
                    }
                }

                MostrarMensajeModal = true;

                ListaCompanias =
                    _service.ObtenerCompanias();

                Compania = new Compania();

                MostrarFormulario = false;

                return Page();
            }
            catch (Exception ex)
            {
                Error =
                    "Ocurrió un error al guardar la compañía.";

                MostrarMensajeModal = true;

                ListaCompanias =
                    _service.ObtenerCompanias();

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error guardando compañía: " + ex.Message);

                return Page();
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login",
                    new { expirada = true });
            }

            try
            {
                string resultado =
                    _service.EliminarCompania(id);

                if (resultado == "OK")
                {
                    Mensaje =
                        "La compañía ha sido eliminada correctamente.";

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Eliminó compañía ID {id}.");
                }
                else
                {
                    Error = resultado;
                }

                MostrarMensajeModal = true;

                ListaCompanias =
                    _service.ObtenerCompanias();

                return Page();
            }
            catch (Exception ex)
            {
                Error =
                    "Ocurrió un error al eliminar la compañía.";

                MostrarMensajeModal = true;

                ListaCompanias =
                    _service.ObtenerCompanias();

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error eliminando compañía: " + ex.Message);

                return Page();
            }
        }

        public JsonResult OnGetCompania(int id)
        {
            var compania =
                _service.ObtenerCompanias()
                .FirstOrDefault(x =>
                    x.IdCompania == id);

            return new JsonResult(compania);
        }
    }
}