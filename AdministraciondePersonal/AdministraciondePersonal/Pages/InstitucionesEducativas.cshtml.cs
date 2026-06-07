using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class InstitucionesEducativasModel : PageModel
    {
        private readonly InstitucionEducativaService _institucionService;
        private readonly BitacoraService _bitacoraService;

        public InstitucionesEducativasModel(
            InstitucionEducativaService institucionService,
            BitacoraService bitacoraService)
        {
            _institucionService = institucionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public InstitucionEducativa Institucion { get; set; } = new InstitucionEducativa();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<InstitucionEducativa> ListaInstituciones { get; set; } = new List<InstitucionEducativa>();

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

        public IActionResult OnGet(int? idInstitucion)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarDatos();

                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "El usuario consulta instituciones educativas.");

                if (idInstitucion.HasValue && idInstitucion.Value > 0)
                {
                    var institucionEncontrada =
                        _institucionService.ObtenerPorId(idInstitucion.Value);

                    if (institucionEncontrada != null)
                    {
                        Institucion = institucionEncontrada;
                        ModoEdicion = true;
                    }
                    else
                    {
                        Error = "La institución educativa seleccionada no existe.";
                        Institucion = new InstitucionEducativa();
                        ModoEdicion = false;
                    }
                }
                else
                {
                    Institucion = new InstitucionEducativa();
                    ModoEdicion = false;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar instituciones educativas: " + ex.Message);

                Error = "Ocurrió un error al consultar las instituciones educativas.";
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
                string resultado =
                    _institucionService.Registrar(Institucion);

                if (resultado == "La institución educativa ha sido registrada correctamente.")
                {
                    Mensaje = resultado;

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Registro de institución educativa '{Institucion.NombreInstitucion}'.");

                    Institucion = new InstitucionEducativa();
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
                    "Error técnico al registrar institución educativa: " + ex.Message);

                Error = "Ocurrió un error al registrar la institución educativa.";
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
                var institucionAnterior =
                    _institucionService.ObtenerPorId(Institucion.IdInstitucion);

                string resultado =
                    _institucionService.Actualizar(Institucion);

                if (resultado == "La institución educativa ha sido actualizada correctamente.")
                {
                    Mensaje = resultado;

                    if (institucionAnterior != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de institución educativa con código {Institucion.IdInstitucion}. " +
                            $"Antes: nombre '{institucionAnterior.NombreInstitucion}'. " +
                            $"Ahora: nombre '{Institucion.NombreInstitucion}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización de institución educativa '{Institucion.NombreInstitucion}' con código {Institucion.IdInstitucion}.");
                    }

                    Institucion = new InstitucionEducativa();
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
                    "Error técnico al actualizar institución educativa: " + ex.Message);

                Error = "Ocurrió un error al actualizar la institución educativa.";
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int idInstitucion)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var institucionEliminada =
                    _institucionService.ObtenerPorId(idInstitucion);

                string resultado =
                    _institucionService.Eliminar(idInstitucion);

                if (resultado == "La institución educativa ha sido eliminada correctamente.")
                {
                    Mensaje = resultado;

                    if (institucionEliminada != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de institución educativa '{institucionEliminada.NombreInstitucion}' con código {institucionEliminada.IdInstitucion}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación de institución educativa con código {idInstitucion}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                Institucion = new InstitucionEducativa();
                ModoEdicion = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar institución educativa: " + ex.Message);

                Error = "Ocurrió un error al eliminar la institución educativa.";
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            ListaInstituciones =
                _institucionService.ObtenerTodos();
        }
    }
}