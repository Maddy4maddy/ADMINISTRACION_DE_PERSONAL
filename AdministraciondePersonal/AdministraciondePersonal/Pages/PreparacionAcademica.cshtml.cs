using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class PreparacionAcademicaModel : PageModel
    {
        private readonly PreparacionAcademicaService _preparacionService;
        private readonly BitacoraService _bitacoraService;

        public PreparacionAcademicaModel(
            PreparacionAcademicaService preparacionService,
            BitacoraService bitacoraService)
        {
            _preparacionService = preparacionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public PreparacionAcademica Preparacion { get; set; } = new PreparacionAcademica();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        [BindProperty]
        public string IdentificacionOferenteSeleccionado { get; set; }

        public bool MostrarFormulario { get; set; }
        public bool MostrarMensajeModal { get; set; }

        public List<PreparacionAcademica> ListaPreparaciones { get; set; } = new List<PreparacionAcademica>();
        public List<InstitucionEducativa> Instituciones { get; set; } = new List<InstitucionEducativa>();
        public List<Oferente> Oferentes { get; set; } = new List<Oferente>();

        public string Mensaje { get; set; }
        public string Error { get; set; }

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

        private string ObtenerNombreInstitucion(int idInstitucion)
        {
            var institucion =
                Instituciones.FirstOrDefault(i => i.IdInstitucion == idInstitucion);

            return institucion != null
                ? institucion.NombreInstitucion
                : "Sin institución";
        }

        private string ObtenerNombreOferente(string identificacion)
        {
            var oferente =
                Oferentes.FirstOrDefault(o => o.Identificacion == identificacion);

            return oferente != null
                ? oferente.NombreCompleto
                : identificacion;
        }

        public IActionResult OnGet(int? idPreparacion, string identificacionOferente, bool nuevo = false)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(identificacionOferente))
                {
                    IdentificacionOferenteSeleccionado = identificacionOferente;
                }

                CargarDatos();

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    $"El usuario consulta preparación académica del oferente {IdentificacionOferenteSeleccionado}.");

                if (nuevo)
                {
                    Preparacion = new PreparacionAcademica
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                    MostrarFormulario = true;
                }
                else if (idPreparacion.HasValue && idPreparacion.Value > 0)
                {
                    var preparacionEncontrada =
                        _preparacionService.ObtenerPorId(idPreparacion.Value);

                    if (preparacionEncontrada != null)
                    {
                        Preparacion = preparacionEncontrada;
                        IdentificacionOferenteSeleccionado = preparacionEncontrada.IdentificacionOferente;
                        ModoEdicion = true;
                        MostrarFormulario = true;
                    }
                    else
                    {
                        Error = "La preparación académica seleccionada no existe.";
                        MostrarMensajeModal = true;
                        Preparacion = new PreparacionAcademica();
                        ModoEdicion = false;
                    }
                }
                else
                {
                    Preparacion = new PreparacionAcademica
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                    MostrarFormulario = false;
                }

                CargarDatos();

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar preparación académica: " + ex.Message);

                Error = "Ocurrió un error al consultar la preparación académica.";
                MostrarMensajeModal = true;
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostGuardar()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(IdentificacionOferenteSeleccionado))
                {
                    Preparacion.IdentificacionOferente = IdentificacionOferenteSeleccionado;
                }

                CargarDatos();

                string resultado =
                    _preparacionService.Registrar(Preparacion);

                if (resultado == "La preparación académica ha sido registrada correctamente.")
                {
                    Mensaje = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = false;

                    string nombreInstitucion =
                        ObtenerNombreInstitucion(Preparacion.IdInstitucion);

                    string nombreOferente =
                        ObtenerNombreOferente(Preparacion.IdentificacionOferente);

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Registro de preparación académica del oferente '{nombreOferente}' con identificación {Preparacion.IdentificacionOferente}: título '{Preparacion.TituloObtenido}' en '{nombreInstitucion}'.");

                    Preparacion = new PreparacionAcademica
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
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

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al registrar preparación académica: " + ex.Message);

                Error = "Ocurrió un error al registrar la preparación académica.";
                MostrarMensajeModal = true;
                MostrarFormulario = true;
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostActualizar()
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(IdentificacionOferenteSeleccionado))
                {
                    Preparacion.IdentificacionOferente = IdentificacionOferenteSeleccionado;
                }

                CargarDatos();

                var preparacionAnterior =
                    _preparacionService.ObtenerPorId(Preparacion.IdPreparacion);

                string institucionAnterior =
                    preparacionAnterior != null && !string.IsNullOrWhiteSpace(preparacionAnterior.NombreInstitucion)
                        ? preparacionAnterior.NombreInstitucion
                        : ObtenerNombreInstitucion(preparacionAnterior?.IdInstitucion ?? 0);

                string institucionActual =
                    ObtenerNombreInstitucion(Preparacion.IdInstitucion);

                string resultado =
                    _preparacionService.Actualizar(Preparacion);

                if (resultado == "La preparación académica ha sido actualizada correctamente.")
                {
                    Mensaje = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = false;

                    string nombreOferente =
                        ObtenerNombreOferente(Preparacion.IdentificacionOferente);

                    if (preparacionAnterior != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de preparación académica del oferente '{nombreOferente}' con identificación {Preparacion.IdentificacionOferente}. " +
                            $"Antes: título '{preparacionAnterior.TituloObtenido}', institución '{institucionAnterior}', inicio {preparacionAnterior.FechaInicio:yyyy-MM-dd}, fin {preparacionAnterior.FechaFin:yyyy-MM-dd}. " +
                            $"Ahora: título '{Preparacion.TituloObtenido}', institución '{institucionActual}', inicio {Preparacion.FechaInicio:yyyy-MM-dd}, fin {Preparacion.FechaFin:yyyy-MM-dd}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de preparación académica del oferente '{nombreOferente}' con identificación {Preparacion.IdentificacionOferente}.");
                    }

                    Preparacion = new PreparacionAcademica
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                }
                else
                {
                    Error = resultado;
                    MostrarMensajeModal = true;
                    MostrarFormulario = true;
                    ModoEdicion = true;
                }

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al actualizar preparación académica: " + ex.Message);

                Error = "Ocurrió un error al actualizar la preparación académica.";
                MostrarMensajeModal = true;
                MostrarFormulario = true;
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int idPreparacion)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos();

                var preparacionEliminada =
                    _preparacionService.ObtenerPorId(idPreparacion);

                string resultado =
                    _preparacionService.Eliminar(idPreparacion);

                if (resultado == "La preparación académica ha sido eliminada correctamente.")
                {
                    Mensaje = resultado;
                    MostrarMensajeModal = true;

                    if (preparacionEliminada != null)
                    {
                        string nombreOferente =
                            ObtenerNombreOferente(preparacionEliminada.IdentificacionOferente);

                        string nombreInstitucion =
                            !string.IsNullOrWhiteSpace(preparacionEliminada.NombreInstitucion)
                                ? preparacionEliminada.NombreInstitucion
                                : ObtenerNombreInstitucion(preparacionEliminada.IdInstitucion);

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de preparación académica del oferente '{nombreOferente}' con identificación {preparacionEliminada.IdentificacionOferente}: título '{preparacionEliminada.TituloObtenido}' en '{nombreInstitucion}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de preparación académica con ID {idPreparacion}.");
                    }
                }
                else
                {
                    Error = resultado;
                    MostrarMensajeModal = true;
                }

                Preparacion = new PreparacionAcademica
                {
                    IdentificacionOferente = IdentificacionOferenteSeleccionado
                };

                ModoEdicion = false;
                MostrarFormulario = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar preparación académica: " + ex.Message);

                Error = "Ocurrió un error al eliminar la preparación académica.";
                MostrarMensajeModal = true;
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            Instituciones =
                _preparacionService.ObtenerInstituciones();

            Oferentes =
                _preparacionService.ObtenerOferentes();

            ListaPreparaciones =
                _preparacionService.ObtenerPorOferente(IdentificacionOferenteSeleccionado);
        }
    }
}