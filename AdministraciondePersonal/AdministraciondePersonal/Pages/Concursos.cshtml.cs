using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class ConcursosModel : PageModel
    {
        private readonly ConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public ConcursosModel(
            ConcursoService concursoService,
            BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public Concurso Concurso { get; set; } = new Concurso();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<Concurso> ListaConcursos { get; set; } = new List<Concurso>();

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

        public IActionResult OnGet(int? codigoConcurso)
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
                    "El usuario consulta concursos.");

                if (codigoConcurso.HasValue && codigoConcurso.Value > 0)
                {
                    var concursoEncontrado =
                        _concursoService.ObtenerPorCodigo(codigoConcurso.Value);

                    if (concursoEncontrado != null)
                    {
                        Concurso = concursoEncontrado;
                        ModoEdicion = true;
                    }
                    else
                    {
                        Error = "El concurso seleccionado no existe.";
                        Concurso = new Concurso();
                        ModoEdicion = false;
                    }
                }
                else
                {
                    Concurso = new Concurso
                    {
                        Estado = "Vigente"
                    };

                    ModoEdicion = false;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar concursos: " + ex.Message);

                Error = "Ocurrió un error al consultar los concursos.";
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
                    _concursoService.Registrar(Concurso);

                if (resultado == "El concurso ha sido registrado correctamente.")
                {
                    Mensaje = resultado;

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Registro de concurso '{Concurso.NombreConcurso}' con código {Concurso.CodigoConcurso}.");

                    Concurso = new Concurso
                    {
                        Estado = "Vigente"
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
                    "Error técnico al registrar concurso: " + ex.Message);

                Error = "Ocurrió un error al registrar el concurso.";
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
                var concursoAnterior =
                    _concursoService.ObtenerPorCodigo(Concurso.CodigoConcurso);

                string resultado =
                    _concursoService.Actualizar(Concurso);

                if (resultado == "El concurso ha sido actualizado correctamente.")
                {
                    Mensaje = resultado;

                    if (concursoAnterior != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización del concurso '{Concurso.NombreConcurso}' con código {Concurso.CodigoConcurso}. " +
                            $"Datos anteriores: nombre '{concursoAnterior.NombreConcurso}', inicio {concursoAnterior.FechaInicio:yyyy-MM-dd}, fin {concursoAnterior.FechaFin:yyyy-MM-dd}, estado '{concursoAnterior.Estado}'. " +
                            $"Datos actuales: nombre '{Concurso.NombreConcurso}', inicio {Concurso.FechaInicio:yyyy-MM-dd}, fin {Concurso.FechaFin:yyyy-MM-dd}, estado '{Concurso.Estado}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización del concurso '{Concurso.NombreConcurso}' con código {Concurso.CodigoConcurso}.");
                    }

                    Concurso = new Concurso
                    {
                        Estado = "Vigente"
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
                    "Error técnico al actualizar concurso: " + ex.Message);

                Error = "Ocurrió un error al actualizar el concurso.";
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(int codigoConcurso)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var concursoEliminado =
                    _concursoService.ObtenerPorCodigo(codigoConcurso);

                string resultado =
                    _concursoService.Eliminar(codigoConcurso);

                if (resultado == "El concurso ha sido eliminado correctamente.")
                {
                    Mensaje = resultado;

                    if (concursoEliminado != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación del concurso '{concursoEliminado.NombreConcurso}' con código {concursoEliminado.CodigoConcurso}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación del concurso con código {codigoConcurso}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                Concurso = new Concurso
                {
                    Estado = "Vigente"
                };

                ModoEdicion = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar concurso: " + ex.Message);

                Error = "Ocurrió un error al eliminar el concurso.";
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostCambiarEstado(int codigoConcurso)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var concursoAnterior =
                    _concursoService.ObtenerPorCodigo(codigoConcurso);

                string resultado =
                    _concursoService.CambiarEstado(codigoConcurso);

                var concursoActual =
                    _concursoService.ObtenerPorCodigo(codigoConcurso);

                if (resultado == "El estado del concurso ha sido actualizado correctamente.")
                {
                    Mensaje = resultado;

                    if (concursoAnterior != null && concursoActual != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Cambio de estado del concurso '{concursoActual.NombreConcurso}' de '{concursoAnterior.Estado}' a '{concursoActual.Estado}'.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Cambio de estado del concurso con código {codigoConcurso}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                Concurso = new Concurso
                {
                    Estado = "Vigente"
                };

                ModoEdicion = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al cambiar estado de concurso: " + ex.Message);

                Error = "Ocurrió un error al cambiar el estado del concurso.";
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            ListaConcursos =
                _concursoService.ObtenerTodos();
        }
    }
}