using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdministraciondePersonal.Pages
{
    public class ConcursosModel : PageModel
    {
        private readonly ConcursoService _concursoService;

        public ConcursosModel(ConcursoService concursoService)
        {
            _concursoService = concursoService;
        }

        [BindProperty]
        public Concurso Concurso { get; set; } = new Concurso();

        [BindProperty]
        public bool ModoEdicion { get; set; }

        public List<Concurso> ListaConcursos { get; set; } = new List<Concurso>();

        public string Mensaje { get; set; }

        public string Error { get; set; }

        public void OnGet(int? codigoConcurso)
        {
            CargarDatos();

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
        }

        public void OnPostGuardar()
        {
            string resultado =
                _concursoService.Registrar(Concurso);

            if (resultado == "El concurso ha sido registrado correctamente.")
            {
                Mensaje = resultado;

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
        }

        public void OnPostActualizar()
        {
            string resultado =
                _concursoService.Actualizar(Concurso);

            if (resultado == "El concurso ha sido actualizado correctamente.")
            {
                Mensaje = resultado;

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
        }

        public void OnPostEliminar(int codigoConcurso)
        {
            string resultado =
                _concursoService.Eliminar(codigoConcurso);

            if (resultado == "El concurso ha sido eliminado correctamente.")
            {
                Mensaje = resultado;
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
        }

        public void OnPostCambiarEstado(int codigoConcurso)
        {
            string resultado =
                _concursoService.CambiarEstado(codigoConcurso);

            if (resultado == "El estado del concurso ha sido actualizado correctamente.")
            {
                Mensaje = resultado;
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
        }

        private void CargarDatos()
        {
            ListaConcursos =
                _concursoService.ObtenerTodos();
        }
    }
}