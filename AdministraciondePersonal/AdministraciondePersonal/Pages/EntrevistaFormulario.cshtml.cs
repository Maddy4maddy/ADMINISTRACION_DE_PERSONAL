using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class EntrevistaFormularioModel : PageModel
    {
        private readonly EntrevistaService _entrevistaService;
        private readonly BitacoraService _bitacoraService;

        public EntrevistaFormularioModel(
            EntrevistaService entrevistaService,
            BitacoraService bitacoraService)
        {
            _entrevistaService = entrevistaService;
            _bitacoraService = bitacoraService;
        }

        public List<Oferente> Oferentes { get; set; } = new List<Oferente>();

        public List<Usuario> Entrevistadores { get; set; } = new List<Usuario>();

        [BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public bool ModoEdicion { get; set; }

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

        private string ObtenerNombreEntrevistador(int idUsuario)
        {
            var entrevistador =
                Entrevistadores.FirstOrDefault(e => e.IdUsuario == idUsuario);

            return entrevistador != null
                ? entrevistador.NombreCompleto
                : "Sin entrevistador";
        }

        public IActionResult OnGet(int? idEntrevista)
        {
            if (!PrepararSesion())
            {
                return RedirectToPage("/Login", new { expirada = true });
            }

            try
            {
                CargarListas();

                if (idEntrevista.HasValue && idEntrevista.Value > 0)
                {
                    var entrevistaEncontrada =
                        _entrevistaService.ObtenerPorId(idEntrevista.Value);

                    if (entrevistaEncontrada != null)
                    {
                        Entrevista = entrevistaEncontrada;
                        ModoEdicion = true;

                        _bitacoraService.RegistrarAccion(
                            NombreUsuario,
                            $"El usuario consulta los datos de la entrevista del oferente '{Entrevista.NombreOferente}' programada para {Entrevista.FechaEntrevista:yyyy-MM-dd HH:mm}.");
                    }
                    else
                    {
                        Error = "La entrevista seleccionada no existe.";
                        Entrevista = new Entrevista();
                        ModoEdicion = false;
                    }
                }
                else
                {
                    Entrevista = new Entrevista
                    {
                        Estado = "Pendiente"
                    };

                    ModoEdicion = false;

                    _bitacoraService.RegistrarAccion(
                        NombreUsuario,
                        "El usuario ingresa a la pantalla para agendar una nueva entrevista.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al consultar formulario de entrevista: " + ex.Message);

                Error = "Ocurrió un error al consultar el formulario de entrevista.";
                CargarListas();
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
                CargarListas();

                string resultado =
                    _entrevistaService.Registrar(Entrevista);

                if (resultado == "La entrevista ha sido agendada correctamente.")
                {
                    Mensaje = resultado;

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
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al agendar entrevista: " + ex.Message);

                Error = "Ocurrió un error al agendar la entrevista.";
                CargarListas();
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
                CargarListas();

                var entrevistaAnterior =
                    _entrevistaService.ObtenerPorId(Entrevista.IdEntrevista);

                if (entrevistaAnterior != null)
                {
                    Entrevista.IdentificacionOferente =
                        entrevistaAnterior.IdentificacionOferente;
                }

                string resultado =
                    _entrevistaService.Actualizar(Entrevista);

                if (resultado == "La entrevista ha sido actualizada correctamente.")
                {
                    Mensaje = resultado;
                    ModoEdicion = true;

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
                    ModoEdicion = true;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _bitacoraService.RegistrarAccion(
                    NombreUsuario,
                    "Error técnico al actualizar entrevista: " + ex.Message);

                Error = "Ocurrió un error al actualizar la entrevista.";
                CargarListas();
                ModoEdicion = true;
                return Page();
            }
        }

        private void CargarListas()
        {
            Oferentes =
                _entrevistaService.ObtenerOferentes();

            Entrevistadores =
                _entrevistaService.ObtenerEntrevistadores();
        }
    }
}