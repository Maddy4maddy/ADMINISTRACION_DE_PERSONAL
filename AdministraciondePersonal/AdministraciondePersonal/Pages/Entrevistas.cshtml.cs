using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class EntrevistasModel : PageModel
    {
        private readonly EntrevistaService _entrevistaService;
        private readonly BitacoraService _bitacoraService;

        public EntrevistasModel(
            EntrevistaService entrevistaService,
            BitacoraService bitacoraService)
        {
            _entrevistaService = entrevistaService;
            _bitacoraService = bitacoraService;
        }

        public List<Entrevista> ListaEntrevistas { get; set; } = new List<Entrevista>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public int PaginaActual { get; set; }

        public int TotalPaginas { get; set; }

        public int TamanioPagina { get; set; } = 10;

        public string NombreUsuario { get; set; }

        public string InicialAvatar { get; set; }

        public string ColorAvatar { get; set; }

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

        public IActionResult OnGet(int pagina = 1)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos(pagina);

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "El usuario consulta entrevistas agendadas.");

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar entrevistas: " + ex.Message);

                Error = "Ocurrió un error al consultar las entrevistas.";
                CargarDatos(pagina);
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int idEntrevista, int paginaActual = 1)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var entrevistaEliminada =
                    _entrevistaService.ObtenerPorId(idEntrevista);

                string resultado =
                    _entrevistaService.Eliminar(idEntrevista);

                if (resultado == "La entrevista ha sido eliminada correctamente.")
                {
                    Mensaje = resultado;

                    if (entrevistaEliminada != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de entrevista del oferente '{entrevistaEliminada.NombreOferente}' con identificación {entrevistaEliminada.IdentificacionOferente}, programada para {entrevistaEliminada.FechaEntrevista:yyyy-MM-dd HH:mm}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de entrevista con ID {idEntrevista}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                CargarDatos(paginaActual);
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar entrevista: " + ex.Message);

                Error = "Ocurrió un error al eliminar la entrevista.";
                CargarDatos(paginaActual);
                return Page();
            }
        }

        public IActionResult OnPostMarcarRealizada(int idEntrevista, int paginaActual = 1)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var entrevistaAnterior =
                    _entrevistaService.ObtenerPorId(idEntrevista);

                string resultado =
                    _entrevistaService.MarcarComoRealizada(idEntrevista);

                var entrevistaActual =
                    _entrevistaService.ObtenerPorId(idEntrevista);

                if (resultado == "La entrevista ha sido marcada como realizada.")
                {
                    Mensaje = resultado;

                    if (entrevistaAnterior != null && entrevistaActual != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Cambio de estado de entrevista del oferente '{entrevistaActual.NombreOferente}' con identificación {entrevistaActual.IdentificacionOferente}: de '{entrevistaAnterior.Estado}' a '{entrevistaActual.Estado}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"La entrevista con ID {idEntrevista} fue marcada como realizada.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                CargarDatos(paginaActual);
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al marcar entrevista como realizada: " + ex.Message);

                Error = "Ocurrió un error al marcar la entrevista como realizada.";
                CargarDatos(paginaActual);
                return Page();
            }
        }

        private void CargarDatos(int pagina)
        {
            if (pagina <= 0)
                pagina = 1;

            PaginaActual = pagina;

            int totalRegistros = _entrevistaService.ContarEntrevistas();

            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)TamanioPagina);

            if (TotalPaginas == 0)
                TotalPaginas = 1;

            ListaEntrevistas = _entrevistaService.ObtenerPaginado(PaginaActual, TamanioPagina);
        }
    }
}