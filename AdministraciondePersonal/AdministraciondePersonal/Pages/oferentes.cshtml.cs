using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class OferentesModel : PageModel
    {
        private readonly OferenteService _oferenteService;

        public OferentesModel(OferenteService oferenteService)
        {
            _oferenteService = oferenteService;
        }

        [BindProperty]
        public Oferente Oferente { get; set; } = new Oferente();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<Oferente> ListaOferentes { get; set; } = new List<Oferente>();

        public List<Concurso> Concursos { get; set; } = new List<Concurso>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(string identificacion)
        {
            CargarDatos();

            if (!string.IsNullOrWhiteSpace(identificacion))
            {
                var oferenteEncontrado =
                    _oferenteService.ObtenerPorIdentificacion(identificacion);

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
        }

        public void OnPostGuardar()
        {
            string resultado =
                _oferenteService.Registrar(Oferente);

            if (resultado == "El oferente ha sido registrado correctamente.")
            {
                Mensaje = resultado;
                Oferente = new Oferente();
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
                _oferenteService.Actualizar(Oferente);

            if (resultado == "El oferente ha sido actualizado correctamente.")
            {
                Mensaje = resultado;
                Oferente = new Oferente();
                ModoEdicion = false;
            }
            else
            {
                Error = resultado;
                ModoEdicion = true;
            }

            CargarDatos();
        }

        public void OnPostEliminar(string identificacion)
        {
            string resultado =
                _oferenteService.Eliminar(identificacion);

            if (resultado == "El oferente ha sido eliminado correctamente.")
            {
                Mensaje = resultado;
            }
            else
            {
                Error = resultado;
            }

            Oferente = new Oferente();
            ModoEdicion = false;

            CargarDatos();
        }

        private void CargarDatos()
        {
            ListaOferentes =
                _oferenteService.ObtenerTodos();

            Concursos =
                _oferenteService.ObtenerConcursos();
        }
    }
}