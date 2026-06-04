using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class BienvenidaModel : PageModel
    {
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }

        public IActionResult OnGet()
        {
            
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            
            NombreUsuario = HttpContext.Session.GetString("Usuario");
            NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (string.IsNullOrEmpty(NombreCompleto))
            {
                NombreCompleto = NombreUsuario;
            }

            
            InicialAvatar = NombreCompleto.Length > 0 ? NombreCompleto.Substring(0, 1).ToUpper() : "U";

        
            int hash = 0;
            foreach (char c in NombreUsuario)
            {
                hash = c + ((hash << 5) - hash);
            }
            var colores = new[] { "#273a77", "#80B0AA", "#FDB3CA", "#315855", "#4A90E2", "#E74C3C", "#2ECC71", "#F39C12", "#9B59B6", "#1ABC9C", "#E67E22", "#3498DB" };
            ColorAvatar = colores[Math.Abs(hash) % colores.Length];

            return Page();
        }
    }
}