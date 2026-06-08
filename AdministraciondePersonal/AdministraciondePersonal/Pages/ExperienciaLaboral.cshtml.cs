using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class ExperienciaLaboralModel : PageModel
    {
        private readonly ExperienciaLaboralService _experienciaService;
        private readonly PreparacionAcademicaService _preparacionService;
        private readonly BitacoraService _bitacoraService;

        public ExperienciaLaboralModel(
            ExperienciaLaboralService experienciaService,
            PreparacionAcademicaService preparacionService,
            BitacoraService bitacoraService)
        {
            _experienciaService = experienciaService;
            _preparacionService = preparacionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public ExperienciaLaboral Experiencia { get; set; } = new ExperienciaLaboral();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        [BindProperty]
        public string IdentificacionOferenteSeleccionado { get; set; }

        public List<ExperienciaLaboral> ListaExperiencias { get; set; } = new List<ExperienciaLaboral>();

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

        private string ObtenerNombreOferente(string identificacion)
        {
            var oferente =
                Oferentes.FirstOrDefault(o => o.Identificacion == identificacion);

            return oferente != null
                ? oferente.NombreCompleto
                : identificacion;
        }

        public IActionResult OnGet(int? idExperiencia, string identificacionOferente)
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
                    $"El usuario consulta experiencia laboral del oferente {IdentificacionOferenteSeleccionado}.");

                if (idExperiencia.HasValue && idExperiencia.Value > 0)
                {
                    var experienciaEncontrada =
                        _experienciaService.ObtenerPorId(idExperiencia.Value);

                    if (experienciaEncontrada != null)
                    {
                        Experiencia = experienciaEncontrada;
                        IdentificacionOferenteSeleccionado = experienciaEncontrada.IdentificacionOferente;
                        ModoEdicion = true;
                    }
                    else
                    {
                        Error = "La experiencia laboral seleccionada no existe.";
                        Experiencia = new ExperienciaLaboral();
                        ModoEdicion = false;
                    }
                }
                else
                {
                    Experiencia = new ExperienciaLaboral
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                }

                CargarDatos();

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar experiencia laboral: " + ex.Message);

                Error = "Ocurrió un error al consultar la experiencia laboral.";
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
                    Experiencia.IdentificacionOferente = IdentificacionOferenteSeleccionado;
                }

                CargarDatos();

                string resultado =
                    _experienciaService.Registrar(Experiencia);

                if (resultado == "La experiencia laboral ha sido registrada correctamente.")
                {
                    Mensaje = resultado;

                    string nombreOferente =
                        ObtenerNombreOferente(Experiencia.IdentificacionOferente);

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Registro de experiencia laboral del oferente '{nombreOferente}' con identificación {Experiencia.IdentificacionOferente}: empresa '{Experiencia.NombreEmpresa}', puesto '{Experiencia.PuestoDesempenado}'.");

                    Experiencia = new ExperienciaLaboral
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                }
                else
                {
                    Error = resultado;
                }

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al registrar experiencia laboral: " + ex.Message);

                Error = "Ocurrió un error al registrar la experiencia laboral.";
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
                    Experiencia.IdentificacionOferente = IdentificacionOferenteSeleccionado;
                }

                CargarDatos();

                var experienciaAnterior =
                    _experienciaService.ObtenerPorId(Experiencia.IdExperiencia);

                string resultado =
                    _experienciaService.Actualizar(Experiencia);

                if (resultado == "La experiencia laboral ha sido actualizada correctamente.")
                {
                    Mensaje = resultado;

                    string nombreOferente =
                        ObtenerNombreOferente(Experiencia.IdentificacionOferente);

                    if (experienciaAnterior != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de experiencia laboral del oferente '{nombreOferente}' con identificación {Experiencia.IdentificacionOferente}. " +
                            $"Antes: empresa '{experienciaAnterior.NombreEmpresa}', puesto '{experienciaAnterior.PuestoDesempenado}', inicio {experienciaAnterior.FechaInicio:yyyy-MM-dd}, fin {experienciaAnterior.FechaFin:yyyy-MM-dd}. " +
                            $"Ahora: empresa '{Experiencia.NombreEmpresa}', puesto '{Experiencia.PuestoDesempenado}', inicio {Experiencia.FechaInicio:yyyy-MM-dd}, fin {Experiencia.FechaFin:yyyy-MM-dd}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de experiencia laboral del oferente '{nombreOferente}' con identificación {Experiencia.IdentificacionOferente}.");
                    }

                    Experiencia = new ExperienciaLaboral
                    {
                        IdentificacionOferente = IdentificacionOferenteSeleccionado
                    };

                    ModoEdicion = false;
                }
                else
                {
                    Error = resultado;
                    ModoEdicion = true;
                }

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al actualizar experiencia laboral: " + ex.Message);

                Error = "Ocurrió un error al actualizar la experiencia laboral.";
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int idExperiencia)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos();

                var experienciaEliminada =
                    _experienciaService.ObtenerPorId(idExperiencia);

                string resultado =
                    _experienciaService.Eliminar(idExperiencia);

                if (resultado == "La experiencia laboral ha sido eliminada correctamente.")
                {
                    Mensaje = resultado;

                    if (experienciaEliminada != null)
                    {
                        string nombreOferente =
                            ObtenerNombreOferente(experienciaEliminada.IdentificacionOferente);

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de experiencia laboral del oferente '{nombreOferente}' con identificación {experienciaEliminada.IdentificacionOferente}: empresa '{experienciaEliminada.NombreEmpresa}', puesto '{experienciaEliminada.PuestoDesempenado}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de experiencia laboral con ID {idExperiencia}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                Experiencia = new ExperienciaLaboral
                {
                    IdentificacionOferente = IdentificacionOferenteSeleccionado
                };

                ModoEdicion = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar experiencia laboral: " + ex.Message);

                Error = "Ocurrió un error al eliminar la experiencia laboral.";
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            Oferentes =
                _preparacionService.ObtenerOferentes();

            ListaExperiencias =
                _experienciaService.ObtenerPorOferente(IdentificacionOferenteSeleccionado);
        }
    }
}