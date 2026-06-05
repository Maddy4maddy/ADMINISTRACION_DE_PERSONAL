using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class ExperienciaLaboralModel : PageModel
    {
        private readonly ExperienciaLaboralService _experienciaService;

        public ExperienciaLaboralModel(ExperienciaLaboralService experienciaService)
        {
            _experienciaService = experienciaService;
        }

        [BindProperty]
        public ExperienciaLaboral Experiencia { get; set; } = new ExperienciaLaboral();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        [BindProperty]
        public string IdentificacionOferenteSeleccionado { get; set; }

        public List<ExperienciaLaboral> ListaExperiencias { get; set; } = new List<ExperienciaLaboral>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(int? idExperiencia, string identificacionOferente)
        {
            if (!string.IsNullOrWhiteSpace(identificacionOferente))
            {
                IdentificacionOferenteSeleccionado = identificacionOferente;
            }

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
        }

        public void OnPostGuardar()
        {
            if (!string.IsNullOrWhiteSpace(IdentificacionOferenteSeleccionado))
            {
                Experiencia.IdentificacionOferente = IdentificacionOferenteSeleccionado;
            }

            string resultado =
                _experienciaService.Registrar(Experiencia);

            if (resultado == "La experiencia laboral ha sido registrada correctamente.")
            {
                Mensaje = resultado;

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
        }

        public void OnPostActualizar()
        {
            if (!string.IsNullOrWhiteSpace(IdentificacionOferenteSeleccionado))
            {
                Experiencia.IdentificacionOferente = IdentificacionOferenteSeleccionado;
            }

            string resultado =
                _experienciaService.Actualizar(Experiencia);

            if (resultado == "La experiencia laboral ha sido actualizada correctamente.")
            {
                Mensaje = resultado;

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
        }

        public void OnPostEliminar(int idExperiencia)
        {
            string resultado =
                _experienciaService.Eliminar(idExperiencia);

            if (resultado == "La experiencia laboral ha sido eliminada correctamente.")
            {
                Mensaje = resultado;
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
        }

        private void CargarDatos()
        {
            ListaExperiencias =
                _experienciaService.ObtenerPorOferente(IdentificacionOferenteSeleccionado);
        }
    }
}