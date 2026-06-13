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

        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }

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

            PrepararSesion();

            ListaRoles = _service.ObtenerRoles();
            Pantallas = _service.ObtenerPantallas() ?? new List<Pantalla>();
        }

        public IActionResult OnPost()
        {
            try
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

                    TempData["Mensaje"] = "Rol creado correctamente.";
                }
                else
                {
                    _service.EditarRol(IdRolEditar, NombreRol, pantallas);

                    _bitacoraService.RegistrarAccion(
                        User.Identity?.Name ?? "Sistema",
                        $"Editó el rol ID {IdRolEditar} - {NombreRol}"
                    );

                    TempData["Mensaje"] = "Rol actualizado correctamente.";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                PrepararSesion();

                ListaRoles = _service.ObtenerRoles();
                Pantallas = _service.ObtenerPantallas() ?? new List<Pantalla>();

                return Page();
            }
        }
        public IActionResult OnPostDelete(int idRol)
        {
            var resultado = _service.EliminarRol(idRol);

            if (resultado == "OK")
            {
                TempData["Mensaje"] = "Rol eliminado correctamente.";
                TempData["EsError"] = "false";
            }
            else
            {
                TempData["Mensaje"] = resultado;
                TempData["EsError"] = "true";
            }

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

            return true;
        }
    }
    
}
