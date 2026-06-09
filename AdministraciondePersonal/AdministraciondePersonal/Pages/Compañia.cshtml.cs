using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class CompaniasModel : PageModel
    {
        private readonly CompaniaService _service;
        private readonly BitacoraService _bitacoraService;

        public CompaniasModel(
            CompaniaService service,
            BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public Compania Compania { get; set; } = new();

        public List<Compania> ListaCompanias { get; set; } = new();

        public void OnGet()
        {
            ModelState.Clear();

            ListaCompanias =
                _service.ObtenerCompanias();
        }

        public IActionResult OnPostGuardar()
        {
            if (string.IsNullOrWhiteSpace(
                Compania.NombreCompania))
            {
                TempData["Mensaje"] =
                    "Todos los datos son requeridos.";

                return RedirectToPage();
            }

            if (Compania.NombreCompania.Length > 150)
            {
                TempData["Mensaje"] =
                    "El nombre no puede superar 150 caracteres.";

                return RedirectToPage();
            }

            if (Compania.IdCompania == 0)
            {
                _service.CrearCompania(
                    Compania.NombreCompania);

                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Creó la compañía: {Compania.NombreCompania}"
                );
            }
            else
            {
                _service.EditarCompania(
                    Compania.IdCompania,
                    Compania.NombreCompania);

                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Editó la compañía ID {Compania.IdCompania} - {Compania.NombreCompania}"
                );
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEliminar(int id)
        {
            var resultado =
                _service.EliminarCompania(id);

            TempData["Mensaje"] = resultado;

            if (resultado == "OK")
            {
                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Eliminó la compañía ID {id}"
                );
            }

            return RedirectToPage();
        }

        public JsonResult OnGetCompania(int id)
        {
            var compania =
                _service.ObtenerCompanias()
                .FirstOrDefault(x => x.IdCompania == id);

            if (compania == null)
            {
                return new JsonResult(null);
            }

            return new JsonResult(new
            {
                idCompania = compania.IdCompania,
                nombreCompania = compania.NombreCompania
            });
        }
    }
}