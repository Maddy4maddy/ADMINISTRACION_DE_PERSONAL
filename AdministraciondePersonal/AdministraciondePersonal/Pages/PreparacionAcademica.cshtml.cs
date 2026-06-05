using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class PreparacionAcademicaModel : PageModel
    {
        private readonly PreparacionAcademicaService _preparacionService;

        public PreparacionAcademicaModel(PreparacionAcademicaService preparacionService)
        {
            _preparacionService = preparacionService;
        }

        [BindProperty]
        public PreparacionAcademica Preparacion { get; set; } = new PreparacionAcademica();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        [BindProperty]
        public string IdentificacionOferenteSeleccionado { get; set; }

        public List<PreparacionAcademica> ListaPreparaciones { get; set; } = new List<PreparacionAcademica>();

        public List<InstitucionEducativa> Instituciones { get; set; } = new List<InstitucionEducativa>();

        public List<Oferente> Oferentes { get; set; } = new List<Oferente>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(int? idPreparacion, string identificacionOferente)
        {
            if (!string.IsNullOrWhiteSpace(identificacionOferente))
            {
                IdentificacionOferenteSeleccionado = identificacionOferente;
            }

            if (idPreparacion.HasValue && idPreparacion.Value > 0)
            {
                var preparacionEncontrada =
                    _preparacionService.ObtenerPorId(idPreparacion.Value);

                if (preparacionEncontrada != null)
                {
                    Preparacion = preparacionEncontrada;
                    IdentificacionOferenteSeleccionado = preparacionEncontrada.IdentificacionOferente;
                    ModoEdicion = true;
                }
                else
                {
                    Error = "La preparación académica seleccionada no existe.";
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
            }

            CargarDatos();
        }

        public void OnPostGuardar()
        {
            if (!string.IsNullOrWhiteSpace(IdentificacionOferenteSeleccionado))
            {
                Preparacion.IdentificacionOferente = IdentificacionOferenteSeleccionado;
            }

            string resultado =
                _preparacionService.Registrar(Preparacion);

            if (resultado == "La preparación académica ha sido registrada correctamente.")
            {
                Mensaje = resultado;

                Preparacion = new PreparacionAcademica
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
                Preparacion.IdentificacionOferente = IdentificacionOferenteSeleccionado;
            }

            string resultado =
                _preparacionService.Actualizar(Preparacion);

            if (resultado == "La preparación académica ha sido actualizada correctamente.")
            {
                Mensaje = resultado;

                Preparacion = new PreparacionAcademica
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

        public void OnPostEliminar(int idPreparacion)
        {
            string resultado =
                _preparacionService.Eliminar(idPreparacion);

            if (resultado == "La preparación académica ha sido eliminada correctamente.")
            {
                Mensaje = resultado;
            }
            else
            {
                Error = resultado;
            }

            Preparacion = new PreparacionAcademica
            {
                IdentificacionOferente = IdentificacionOferenteSeleccionado
            };

            ModoEdicion = false;

            CargarDatos();
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