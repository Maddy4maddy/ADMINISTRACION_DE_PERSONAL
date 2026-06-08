using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Pages
{
    public class RolesModel : PageModel
    {
        private readonly RolService _service;
        private readonly BitacoraService _bitacoraService;

        public RolesModel(RolService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public string NombreRol { get; set; }

        [BindProperty]
        public int IdRolEditar { get; set; }

        [BindProperty]
        public List<int> PantallasSeleccionadas { get; set; } = new();

        public List<rol> ListaRoles { get; set; } = new();

        public List<Pantalla> Pantallas { get; set; } = new();

        public void OnGet()
        {
            ModelState.Clear();

            ListaRoles = _service.ObtenerRoles();
            Pantallas = _service.ObtenerPantallas() ?? new List<Pantalla>();
        }

        public IActionResult OnPost()
        {
            var pantallas = _service.ObtenerPantallas() ?? new List<Pantalla>();

            foreach (var p in pantallas)
            {
                p.Seleccionada = PantallasSeleccionadas.Contains(p.IdPantalla);
            }

            if (IdRolEditar == 0)
            {
                _service.CrearRolConPantallas(NombreRol, pantallas);

                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Creó el rol: {NombreRol}"
                );
            }
            else
            {
                _service.EditarRol(IdRolEditar, NombreRol, pantallas);

                _bitacoraService.RegistrarAccion(
                    User.Identity?.Name ?? "Sistema",
                    $"Editó el rol ID {IdRolEditar} - {NombreRol}"
                );
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int idRol)
        {
            TempData["Mensaje"] = _service.EliminarRol(idRol);

            _bitacoraService.RegistrarAccion(
                User.Identity?.Name ?? "Sistema",
                $"Eliminó el rol ID {idRol}"
            );

            return RedirectToPage();
        }

        public JsonResult OnGetRol(int id)
        {
            var rol = _service.ObtenerRoles()
                .FirstOrDefault(x => x.IdRol == id);

            var pantallasAsignadas = _service.ObtenerPantallasPorRol(id);

            var pantallas = _service.ObtenerPantallas() ?? new List<Pantalla>();

            return new JsonResult(new
            {
                rol = new
                {
                    idRol = rol.IdRol,
                    nombreRol = rol.NombreRol
                },
                pantallas = pantallasAsignadas.Select(idPantalla => new
                {
                    idPantalla
                })
            });
        }
    }
    
}
