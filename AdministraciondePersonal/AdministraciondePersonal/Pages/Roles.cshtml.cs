using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;

namespace AdministraciondePersonal.Pages
{
    public class RolesModel : PageModel
    {
        private readonly RolService _service;

        public RolesModel(RolService service)
        {
            _service = service;
        }

        [BindProperty]
        public string NombreRol { get; set; }

        [BindProperty]
        public int IdRolEditar { get; set; }

        [BindProperty]
        public List<Pantalla> Pantallas { get; set; } = new();

        public List<rol> ListaRoles { get; set; }

        public void OnGet()
        {
            ListaRoles = _service.ObtenerRoles();
            Pantallas = _service.ObtenerPantallas();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(NombreRol))
            {
                ListaRoles = _service.ObtenerRoles();
                Pantallas = _service.ObtenerPantallas();
                return Page();
            }

            _service.CrearRolConPantallas(NombreRol, Pantallas);

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int idRol)
        {
            try
            {
                _service.EliminarRol(idRol);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEditar()
        {
            _service.EditarRol(IdRolEditar, NombreRol);
            return RedirectToPage();
        }
    }
}