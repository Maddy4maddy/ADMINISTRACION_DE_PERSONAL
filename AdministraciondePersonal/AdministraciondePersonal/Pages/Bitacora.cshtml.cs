using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdministraciondePersonal.Pages
{
    public class BitacoraModel : PageModel
    {
        private readonly BitacoraService _bitacoraService;

        public BitacoraModel(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        public List<Bitacora> Bitacoras { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public string OrdenarPor { get; set; } = "FechaBitacora";
        public string Direccion { get; set; } = "DESC";
        public string FiltroUsuario { get; set; }
        public string FiltroDescripcion { get; set; }
        public SelectList UsuariosList { get; set; }

        // Datos para el layout
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }
        public string Mensaje { get; set; }
        public string Error { get; set; }

        public IActionResult OnGet(
            int pageIndex = 1,
            string ordenarPor = "FechaBitacora",
            string direccion = "DESC",
            string filtroUsuario = null,
            string filtroDescripcion = null)
        {
            // Verificar sesión
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            // Obtener datos del usuario
            NombreUsuario = HttpContext.Session.GetString("Usuario");
            InicialAvatar = NombreUsuario?.Substring(0, 1).ToUpper();

            // Generar color para avatar
            int hash = 0;
            foreach (char c in NombreUsuario ?? "U")
                hash = c + ((hash << 5) - hash);
            var colores = new[] { "#273a77", "#80B0AA", "#FDB3CA", "#315855", "#4A90E2", "#E74C3C" };
            ColorAvatar = colores[Math.Abs(hash) % colores.Length];

            PageIndex = pageIndex;
            OrdenarPor = ordenarPor;
            Direccion = direccion;
            FiltroUsuario = filtroUsuario;
            FiltroDescripcion = filtroDescripcion;

            var result = _bitacoraService.ObtenerBitacoras(
                pageIndex: pageIndex,
                pageSize: 100,
                ordenarPor: ordenarPor,
                direccion: direccion,
                filtroUsuario: filtroUsuario,
                filtroDescripcion: filtroDescripcion);

            Bitacoras = result.items;
            TotalCount = result.totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalCount / 100);

            // Cargar lista de usuarios para el filtro
            var usuarios = _bitacoraService.ObtenerUsuariosUnicos();
            UsuariosList = new SelectList(usuarios);

            return Page();
        }
    }
}