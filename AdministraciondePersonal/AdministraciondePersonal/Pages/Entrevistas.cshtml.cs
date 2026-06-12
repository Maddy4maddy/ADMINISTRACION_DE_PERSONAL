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
        public List<Oferente> Oferentes { get; set; } = new List<Oferente>();
        public List<Usuario> Entrevistadores { get; set; } = new List<Usuario>();

        [BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public bool ModoEdicion { get; set; }
        public bool MostrarFormulario { get; set; }
        public bool MostrarMensajeModal { get; set; }

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

        private string ObtenerNombreOferente(string identificacion)
        {
            var oferente =
                Oferentes.FirstOrDefault(o => o.Identificacion == identificacion);

            return oferente != null
                ? oferente.NombreCompleto
                : identificacion;
        }

        private string ObtenerNombreEntrevistador(int idUsuario)
        {
            var entrevistador =
                Entrevistadores.FirstOrDefault(e => e.IdUsuario == idUsuario);

            return entrevistador != null
                ? entrevistador.NombreCompleto
                : "Sin entrevistador";
        }

        public IActionResult OnGet(int pagina = 1, bool nuevo = false, int? idEntrevista = null)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos(pagina);
                CargarListas();

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "El usuario consulta entrevistas agendadas.");

                if (nuevo)
                {
                    Entrevista = new Entrevista
                    {
                        Estado = "Pendiente"
                    };

                    ModoEdicion = false;
                    MostrarFormulario = true;
                }
                else if (idEntrevista.HasValue && idEntrevista.Value > 0)
                {
                    var entrevistaEncontrada =
                        _entrevistaService.ObtenerPorId(idEntrevista.Value);

                    if (entrevistaEncontrada != null)
                    {
                        Entrevista = entrevistaEncontrada;
                        ModoEdicion = true;
                        MostrarFormulario = true;

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"El usuario consulta los datos de la entrevista del oferente '{Entrevista.NombreOferente}' programada para {Entrevista.FechaEntrevista:yyyy-MM-dd HH:mm}.");
                    }
                    else
                    {
                        Error = "La entrevista seleccionada no existe.";
                        MostrarMensajeModal = true;
                        Entrevista = new Entrevista();
                        ModoEdicion = false;
                        MostrarFormulario = false;
                    }
                }
                else
                {
                    Entrevista = new Entrevista
                    {
                        Estado = "Pendiente"
                    };

                    ModoEdicion = false;
                    MostrarFormulario = false;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar entrevistas: " + ex.Message);

                Error = "Ocurrió un error al consultar las entrevistas.";
                MostrarMensajeModal = true;
                CargarDatos(pagina);
                CargarListas();
                return Page();
            }
        }

        public IActionResult OnPostGuardar(int paginaActual = 1)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos(paginaActual);
                CargarListas();

                Entrevista.Estado = "Pendiente";

                string resultado =
                    _entrevistaService.Registrar(Entrevista);

                if (resultado == "La entrevista ha sido agendada correctamente.")
                {
                    Mensaje = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = false;

                    string nombreOferente =
                        ObtenerNombreOferente(Entrevista.IdentificacionOferente);

                    string nombreEntrevistador =
                        ObtenerNombreEntrevistador(Entrevista.IdUsuarioEntrevistador);

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Agenda de nueva entrevista para el oferente '{nombreOferente}' con identificación {Entrevista.IdentificacionOferente}, entrevistador '{nombreEntrevistador}', fecha {Entrevista.FechaEntrevista:yyyy-MM-dd HH:mm}, estado 'Pendiente'.");

                    Entrevista = new Entrevista
                    {
                        Estado = "Pendiente"
                    };

                    ModoEdicion = false;
                }
                else
                {
                    Error = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = true;
                    ModoEdicion = false;
                }

                CargarDatos(paginaActual);
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al agendar entrevista: " + ex.Message);

                Error = "Ocurrió un error al agendar la entrevista.";
                MostrarMensajeModal = true;
                MostrarFormulario = true;
                CargarDatos(paginaActual);
                CargarListas();
                return Page();
            }
        }

        public IActionResult OnPostActualizar(int paginaActual = 1)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos(paginaActual);
                CargarListas();

                var entrevistaAnterior =
                    _entrevistaService.ObtenerPorId(Entrevista.IdEntrevista);

                if (entrevistaAnterior != null)
                {
                    Entrevista.IdentificacionOferente =
                        entrevistaAnterior.IdentificacionOferente;

                    Entrevista.NombreOferente =
                        entrevistaAnterior.NombreOferente;
                }

                string resultado =
                    _entrevistaService.Actualizar(Entrevista);

                if (resultado == "La entrevista ha sido actualizada correctamente.")
                {
                    Mensaje = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = false;
                    ModoEdicion = false;

                    string nombreOferente =
                        entrevistaAnterior != null && !string.IsNullOrWhiteSpace(entrevistaAnterior.NombreOferente)
                            ? entrevistaAnterior.NombreOferente
                            : ObtenerNombreOferente(Entrevista.IdentificacionOferente);

                    string entrevistadorAnterior =
                        entrevistaAnterior != null && !string.IsNullOrWhiteSpace(entrevistaAnterior.NombreEntrevistador)
                            ? entrevistaAnterior.NombreEntrevistador
                            : "Sin entrevistador";

                    string entrevistadorActual =
                        ObtenerNombreEntrevistador(Entrevista.IdUsuarioEntrevistador);

                    if (entrevistaAnterior != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de entrevista del oferente '{nombreOferente}' con identificación {Entrevista.IdentificacionOferente}. " +
                            $"Antes: entrevistador '{entrevistadorAnterior}', fecha {entrevistaAnterior.FechaEntrevista:yyyy-MM-dd HH:mm}. " +
                            $"Ahora: entrevistador '{entrevistadorActual}', fecha {Entrevista.FechaEntrevista:yyyy-MM-dd HH:mm}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de entrevista con ID {Entrevista.IdEntrevista}.");
                    }
                }
                else
                {
                    Error = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = true;
                    ModoEdicion = true;
                }

                CargarDatos(paginaActual);
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al actualizar entrevista: " + ex.Message);

                Error = "Ocurrió un error al actualizar la entrevista.";
                MostrarMensajeModal = true;
                MostrarFormulario = true;
                ModoEdicion = true;
                CargarDatos(paginaActual);
                CargarListas();
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
                    MostrarMensajeModal = true;

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
                    MostrarMensajeModal = true;
                }

                Entrevista = new Entrevista
                {
                    Estado = "Pendiente"
                };

                ModoEdicion = false;
                MostrarFormulario = false;

                CargarDatos(paginaActual);
                CargarListas();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar entrevista: " + ex.Message);

                Error = "Ocurrió un error al eliminar la entrevista.";
                MostrarMensajeModal = true;
                CargarDatos(paginaActual);
                CargarListas();
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
                    MostrarMensajeModal = true;

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
                    MostrarMensajeModal = true;
                }

                Entrevista = new Entrevista
                {
                    Estado = "Pendiente"
                };

                ModoEdicion = false;
                MostrarFormulario = false;

                CargarDatos(paginaActual);
                CargarListas();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al marcar entrevista como realizada: " + ex.Message);

                Error = "Ocurrió un error al marcar la entrevista como realizada.";
                MostrarMensajeModal = true;
                CargarDatos(paginaActual);
                CargarListas();
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

        private void CargarListas()
        {
            Oferentes = _entrevistaService.ObtenerOferentes();
            Entrevistadores = _entrevistaService.ObtenerEntrevistadores();
        }
    }
}