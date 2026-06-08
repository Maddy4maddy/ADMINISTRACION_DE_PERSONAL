using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class PantallasModel : PageModel
    {
        private readonly PantallaService _service;

        public PantallasModel(PantallaService service)
        {
            _service = service;
        }

        public List<Pantalla> ListaPantallas { get; set; } = new();

        [BindProperty]
        public int IdPantalla { get; set; }

        [BindProperty]
        public string NombrePantalla { get; set; }

        [BindProperty]
        public string Ruta { get; set; }

        public void OnGet()
        {
            ListaPantallas = _service.ObtenerPantallas();
        }

        public IActionResult OnPost()
        {
            _service.GuardarPantalla(
                IdPantalla,
                NombrePantalla,
                Ruta);

            TempData["Mensaje"] =
                "Pantalla guardada correctamente.";

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                _service.EliminarPantalla(id);

                TempData["Mensaje"] =
                    "Pantalla eliminada correctamente.";
            }
            catch
            {
                TempData["Mensaje"] =
                    "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage();
        }

        public JsonResult OnGetPantalla(int id)
        {
            var pantalla = _service.ObtenerPorId(id);

            return new JsonResult(pantalla);
        }
    }
}
