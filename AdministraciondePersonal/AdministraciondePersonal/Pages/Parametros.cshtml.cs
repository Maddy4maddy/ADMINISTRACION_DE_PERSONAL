using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class ParametrosModel : PageModel
    {
        private readonly ParametroService _service;

        public ParametrosModel(ParametroService service)
        {
            _service = service;
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
            _service.GuardarParametro(
                IdParametro,
                Codigo,
                Valor);

            TempData["Mensaje"] = "Parámetro guardado correctamente.";

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.EliminarParametro(id);

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
