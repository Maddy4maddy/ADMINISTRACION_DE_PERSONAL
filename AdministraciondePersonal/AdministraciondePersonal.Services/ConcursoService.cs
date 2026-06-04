using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class ConcursoService
    {
        private readonly ConcursoRepository _repository;

        public ConcursoService(ConcursoRepository repository)
        {
            _repository = repository;
        }

        public List<Concurso> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public Concurso ObtenerPorCodigo(int codigoConcurso)
        {
            return _repository.ObtenerPorCodigo(codigoConcurso);
        }

        public string Registrar(Concurso concurso)
        {
            string validacion = ValidarConcurso(concurso);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (_repository.ExisteCodigo(concurso.CodigoConcurso))
                return "El código del concurso ya existe.";

            concurso.Estado = "Vigente";

            _repository.Insertar(concurso);

            return "El concurso ha sido registrado correctamente.";
        }

        public string Actualizar(Concurso concurso)
        {
            string validacion = ValidarConcurso(concurso);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (!_repository.ExisteCodigo(concurso.CodigoConcurso))
                return "El concurso seleccionado no existe.";

            _repository.Actualizar(concurso);

            return "El concurso ha sido actualizado correctamente.";
        }

        public string Eliminar(int codigoConcurso)
        {
            if (codigoConcurso <= 0)
                return "Debe seleccionar un concurso para eliminar.";

            if (!_repository.ExisteCodigo(codigoConcurso))
                return "El concurso seleccionado no existe.";

            if (_repository.TieneDatosRelacionados(codigoConcurso))
                return "No se puede eliminar un registro con datos relacionados.";

            _repository.Eliminar(codigoConcurso);

            return "El concurso ha sido eliminado correctamente.";
        }

        public string CambiarEstado(int codigoConcurso)
        {
            var concurso = _repository.ObtenerPorCodigo(codigoConcurso);

            if (concurso == null)
                return "El concurso seleccionado no existe.";

            string nuevoEstado = concurso.Estado == "Vigente" ? "Vencido" : "Vigente";

            _repository.CambiarEstado(codigoConcurso, nuevoEstado);

            return "El estado del concurso ha sido actualizado correctamente.";
        }

        private string ValidarConcurso(Concurso concurso)
        {
            if (concurso == null)
                return "Debe ingresar los datos del concurso.";

            if (concurso.CodigoConcurso <= 0)
                return "El código del concurso es requerido.";

            if (string.IsNullOrWhiteSpace(concurso.NombreConcurso))
                return "El nombre del concurso es requerido.";

            if (concurso.FechaInicio == DateTime.MinValue)
                return "La fecha de inicio es requerida.";

            if (concurso.FechaFin == DateTime.MinValue)
                return "La fecha de fin es requerida.";

            if (concurso.FechaFin < concurso.FechaInicio)
                return "La fecha de fin debe ser mayor o igual a la fecha de inicio.";

            if (string.IsNullOrWhiteSpace(concurso.Estado))
                concurso.Estado = "Vigente";

            if (concurso.Estado != "Vigente" && concurso.Estado != "Vencido")
                return "El estado debe ser Vigente o Vencido.";

            return "";
        }
    }
}