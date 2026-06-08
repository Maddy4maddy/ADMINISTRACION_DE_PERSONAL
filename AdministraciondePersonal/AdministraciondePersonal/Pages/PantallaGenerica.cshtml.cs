using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;

namespace AdministraciondePersonal.Pages
{
    public class PantallaGenericaModel : PageModel
    {
        private readonly PantallaService _service;

        public PantallaGenericaModel(
            PantallaService service)
        {
            _service = service;
        }

        public string NombrePantalla { get; set; }

        public IActionResult OnGet(
            string rutaPersonalizada)
        {
            string rutaBuscada =
                "/" + rutaPersonalizada;

            var pantalla =
                _service.ObtenerPorRuta(rutaBuscada);

            if (pantalla == null)
            {
                return NotFound();
            }

            NombrePantalla =
                pantalla.NombrePantalla;

            return Page();
        }
    }
}