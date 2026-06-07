using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class OferentesModel : PageModel
    {
        private readonly OferenteService _oferenteService;
        private readonly BitacoraService _bitacoraService;

        public OferentesModel(OferenteService oferenteService, BitacoraService bitacoraService)
        {
            _oferenteService = oferenteService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public Oferente Oferente { get; set; } = new Oferente();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<Oferente> ListaOferentes { get; set; } = new List<Oferente>();
        public List<Concurso> Concursos { get; set; } = new List<Concurso>();

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

        private string ObtenerNombreConcurso(int codigoConcurso)
        {
            var concurso = Concursos.FirstOrDefault(c => c.CodigoConcurso == codigoConcurso);
            return concurso != null ? concurso.NombreConcurso : "Sin concurso";
        }

        public IActionResult OnGet(string identificacion)
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
                    "El usuario consulta oferentes.");

                if (!string.IsNullOrWhiteSpace(identificacion))
                {
                    var oferenteEncontrado = _oferenteService.ObtenerPorIdentificacion(identificacion);

                    if (oferenteEncontrado != null)
                    {
                        Oferente = oferenteEncontrado;
                        ModoEdicion = true;
                    }
                    else
                    {
                        Error = "El oferente seleccionado no existe.";
                        ModoEdicion = false;
                        Oferente = new Oferente();
                    }
                }
                else
                {
                    Oferente = new Oferente();
                    ModoEdicion = false;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar oferentes: " + ex.Message);

                Error = "Ocurrió un error al consultar los oferentes.";
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
                CargarDatos();

                string resultado = _oferenteService.Registrar(Oferente);

                if (resultado == "El oferente ha sido registrado correctamente.")
                {
                    Mensaje = resultado;

                    string nombreConcurso = ObtenerNombreConcurso(Oferente.CodigoConcurso);

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        $"Registro de oferente '{Oferente.NombreCompleto}' con identificación {Oferente.Identificacion}, asignado al concurso '{nombreConcurso}'.");

                    Oferente = new Oferente();
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
                    "Error técnico al registrar oferente: " + ex.Message);

                Error = "Ocurrió un error al registrar el oferente.";
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
                CargarDatos();

                var oferenteAnterior =
                    _oferenteService.ObtenerPorIdentificacion(Oferente.Identificacion);

                string concursoAnterior =
                    oferenteAnterior != null && !string.IsNullOrWhiteSpace(oferenteAnterior.NombreConcurso)
                        ? oferenteAnterior.NombreConcurso
                        : ObtenerNombreConcurso(oferenteAnterior?.CodigoConcurso ?? 0);

                string concursoActual =
                    ObtenerNombreConcurso(Oferente.CodigoConcurso);

                string resultado = _oferenteService.Actualizar(Oferente);

                if (resultado == "El oferente ha sido actualizado correctamente.")
                {
                    Mensaje = resultado;

                    if (oferenteAnterior != null)
                    {
                        if (concursoAnterior != concursoActual)
                        {
                            _bitacoraService.RegistrarAccion(
                                NombreUsuario,
                                $"Cambio de concurso del oferente '{Oferente.NombreCompleto}' con identificación {Oferente.Identificacion}: de '{concursoAnterior}' a '{concursoActual}'.");
                        }
                        else
                        {
                            _bitacoraService.RegistrarAccion(
                                NombreUsuario,
                                $"Actualización del oferente '{Oferente.NombreCompleto}' con identificación {Oferente.Identificacion}.");
                        }
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Actualización del oferente '{Oferente.NombreCompleto}' con identificación {Oferente.Identificacion}.");
                    }

                    Oferente = new Oferente();
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
                    "Error técnico al actualizar oferente: " + ex.Message);

                Error = "Ocurrió un error al actualizar el oferente.";
                CargarDatos();
                return Page();
            }
        }

        public IActionResult OnPostEliminar(string identificacion)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                var oferenteEliminado =
                    _oferenteService.ObtenerPorIdentificacion(identificacion);

                string resultado =
                    _oferenteService.Eliminar(identificacion);

                if (resultado == "El oferente ha sido eliminado correctamente.")
                {
                    Mensaje = resultado;

                    if (oferenteEliminado != null)
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación del oferente '{oferenteEliminado.NombreCompleto}' con identificación {oferenteEliminado.Identificacion}.");
                    }
                    else
                    {
                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"Eliminación del oferente con identificación {identificacion}.");
                    }
                }
                else
                {
                    Error = resultado;
                }

                Oferente = new Oferente();
                ModoEdicion = false;

                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al eliminar oferente: " + ex.Message);

                Error = "Ocurrió un error al eliminar el oferente.";
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            ListaOferentes = _oferenteService.ObtenerTodos();
            Concursos = _oferenteService.ObtenerConcursos();
        }
    }
}