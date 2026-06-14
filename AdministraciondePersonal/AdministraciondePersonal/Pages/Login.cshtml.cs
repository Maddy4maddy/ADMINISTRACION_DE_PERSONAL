using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UsuarioService _usuarioService;

        public LoginModel(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string MensajeError { get; set; }
        public bool SesionExpirada { get; set; }

        public void OnGet()
        {
            if (Request.Query.ContainsKey("expirada"))
            {
                SesionExpirada = true;
            }

            if (HttpContext.Session.GetString("Usuario") != null)
            {
                Response.Redirect("/Bienvenida");
            }

            if (Request.Query.ContainsKey("logout"))
            {
                HttpContext.Session.Clear();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contrasenia))
            {
                MensajeError = "Usuario y/o contraseña incorrectos.";
                return Page();
            }

            if (_usuarioService.UsuarioBloqueado(Usuario))
            {
                MensajeError = "Usuario bloqueado por múltiples intentos fallidos.";
                return Page();
            }

            var usuario = await Task.Run(() => _usuarioService.Autenticar(Usuario, Contrasenia));

            if (usuario == null)
            {
                await Task.Run(() => _usuarioService.RegistrarIntentoFallido(Usuario));
                MensajeError = "Usuario y/o contraseña incorrectos.";
                return Page();
            }

            await Task.Run(() => _usuarioService.ResetearIntentos(Usuario));

            HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("NombreCompleto", usuario.NombreCompleto);

            var rolesIds = usuario.Roles
                .Select(r => r.IdRol)
                .ToList();

            HttpContext.Session.SetString(
                "RolesUsuario",
                System.Text.Json.JsonSerializer.Serialize(rolesIds)
            );

            return RedirectToPage("/Bienvenida");
        }
    }
}