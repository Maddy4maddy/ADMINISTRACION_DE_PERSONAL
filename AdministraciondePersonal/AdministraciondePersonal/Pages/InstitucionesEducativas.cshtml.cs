using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class InstitucionesEducativasModel : PageModel
    {
        private readonly InstitucionEducativaService _institucionService;

        public InstitucionesEducativasModel(InstitucionEducativaService institucionService)
        {
            _institucionService = institucionService;
        }

        [BindProperty]
        public InstitucionEducativa Institucion { get; set; } = new InstitucionEducativa();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<InstitucionEducativa> ListaInstituciones { get; set; } = new List<InstitucionEducativa>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(int? idInstitucion)
        {
            CargarDatos();

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
        }

        public void OnPostGuardar()
        {
            string resultado =
                _institucionService.Registrar(Institucion);

            if (resultado == "La institución educativa ha sido registrada correctamente.")
            {
                Mensaje = resultado;
                Institucion = new InstitucionEducativa();
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
            string resultado =
                _institucionService.Actualizar(Institucion);

            if (resultado == "La institución educativa ha sido actualizada correctamente.")
            {
                Mensaje = resultado;
                Institucion = new InstitucionEducativa();
                ModoEdicion = false;
            }
            else
            {
                Error = resultado;
                ModoEdicion = true;
            }

            CargarDatos();
        }

        public void OnPostEliminar(int idInstitucion)
        {
            string resultado =
                _institucionService.Eliminar(idInstitucion);

            if (resultado == "La institución educativa ha sido eliminada correctamente.")
            {
                Mensaje = resultado;
            }
            else
            {
                Error = resultado;
            }

            Institucion = new InstitucionEducativa();
            ModoEdicion = false;

            CargarDatos();
        }

        private void CargarDatos()
        {
            ListaInstituciones =
                _institucionService.ObtenerTodos();
        }
    }
}