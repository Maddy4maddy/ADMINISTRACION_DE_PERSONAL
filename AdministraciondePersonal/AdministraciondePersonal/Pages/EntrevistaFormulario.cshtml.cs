using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class EntrevistaFormularioModel : PageModel
    {
        private readonly EntrevistaService _entrevistaService;

        public EntrevistaFormularioModel(EntrevistaService entrevistaService)
        {
            _entrevistaService = entrevistaService;
        }

        public List<Oferente> Oferentes { get; set; } = new List<Oferente>();

        public List<Usuario> Entrevistadores { get; set; } = new List<Usuario>();

        [Microsoft.AspNetCore.Mvc.BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public bool ModoEdicion { get; set; }

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(int? idEntrevista)
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
            }
        }

        public void OnPostGuardar()
        {
            CargarListas();

            string resultado =
                _entrevistaService.Registrar(Entrevista);

            if (resultado == "La entrevista ha sido agendada correctamente.")
            {
                Mensaje = resultado;

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
        }

        public void OnPostActualizar()
        {
            CargarListas();

            var entrevistaActual =
                _entrevistaService.ObtenerPorId(Entrevista.IdEntrevista);

            if (entrevistaActual != null)
            {
                Entrevista.IdentificacionOferente =
                    entrevistaActual.IdentificacionOferente;
            }

            string resultado =
                _entrevistaService.Actualizar(Entrevista);

            if (resultado == "La entrevista ha sido actualizada correctamente.")
            {
                Mensaje = resultado;
                ModoEdicion = true;
            }
            else
            {
                Error = resultado;
                ModoEdicion = true;
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