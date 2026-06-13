using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdministraciondePersonal.Services;
using AdministraciondePersonal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdministraciondePersonal.Pages
{
    public class BitacoraModel : PageModel
    {
        private readonly BitacoraService _bitacoraService;

        public BitacoraModel(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        // Propiedades del usuario
        public string NombreUsuario { get; set; }
        public string InicialAvatar { get; set; }
        public string ColorAvatar { get; set; }

        // Propiedades para la bitácora
        public List<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();
        public SelectList UsuariosList { get; set; }

        // Propiedades de filtros
        [BindProperty(SupportsGet = true)]
        public string FiltroUsuario { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FiltroDescripcion { get; set; }

        // Propiedades de paginación (igual que Entrevistas)
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanioPagina { get; set; } = 10;

        // Propiedades de ordenamiento
        [BindProperty(SupportsGet = true)]
        public string OrdenarPor { get; set; } = "FechaBitacora";

        [BindProperty(SupportsGet = true)]
        public string Direccion { get; set; } = "DESC";

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

        public IActionResult OnGet(int pagina = 1, string ordenarPor = "FechaBitacora",
                                   string direccion = "DESC", string filtroUsuario = "",
                                   string filtroDescripcion = "")
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            PaginaActual = pagina;
            OrdenarPor = ordenarPor;
            Direccion = direccion;
            FiltroUsuario = filtroUsuario;
            FiltroDescripcion = filtroDescripcion;

            
            var (items, totalCount) = _bitacoraService.ObtenerBitacoras(
                pageIndex: PaginaActual,
                pageSize: TamanioPagina,
                ordenarPor: OrdenarPor,
                direccion: Direccion,
                filtroUsuario: FiltroUsuario,
                filtroDescripcion: FiltroDescripcion
            );

            Bitacoras = items;
            int totalRegistros = totalCount;

            // Calcular total de páginas
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)TamanioPagina);
            if (TotalPaginas == 0) TotalPaginas = 1;

            // Validar página actual
            if (PaginaActual < 1) PaginaActual = 1;
            if (PaginaActual > TotalPaginas) PaginaActual = TotalPaginas;

            // Cargar lista de usuarios para el filtro
            var usuarios = _bitacoraService.ObtenerUsuariosUnicos();
            var itemsLista = new List<SelectListItem>();

            foreach (var usuario in usuarios)
            {
                itemsLista.Add(new SelectListItem { Value = usuario, Text = usuario });
            }

            UsuariosList = new SelectList(itemsLista, "Value", "Text");

            return Page();
        }
    }
}