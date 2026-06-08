using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class ParametrosModel : PageModel
    {
        private readonly ParametroService _service;
        private readonly BitacoraService _bitacoraService;

        public ParametrosModel(ParametroService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        public List<Parametros> ListaParametros { get; set; } = new();

        [BindProperty]
        public int IdParametro { get; set; }

        [BindProperty]
        public string Codigo { get; set; }

        [BindProperty]
        public string Valor { get; set; }

        public void OnGet()
        {
            ListaParametros = _service.ObtenerParametros();
        }
        public IActionResult OnPost()
        {
            _service.GuardarParametro(IdParametro, Codigo, Valor);

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

            TempData["Mensaje"] = "Parámetro guardado correctamente.";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.EliminarParametro(id);

            _bitacoraService.RegistrarAccion(
                User.Identity?.Name ?? "Sistema",
                $"Eliminó el parámetro ID {id}"
            );

            TempData["Mensaje"] = "Parámetro eliminado correctamente.";
            return RedirectToPage();
        }

        public JsonResult OnGetParametro(int id)
        {
            var parametro = _service.ObtenerPorId(id);

            return new JsonResult(parametro);
        }
    }
}
