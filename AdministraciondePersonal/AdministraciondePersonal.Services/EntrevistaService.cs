using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class EntrevistaService
    {
        private readonly EntrevistaRepository _repository;

        public EntrevistaService(EntrevistaRepository repository)
        {
            _repository = repository;
        }

        public List<Entrevista> ObtenerPaginado(int pagina, int tamanioPagina)
        {
            if (pagina <= 0)
                pagina = 1;

            return _repository.ObtenerPaginado(pagina, tamanioPagina);
        }

        public int ContarEntrevistas()
        {
            return _repository.ContarEntrevistas();
        }

        public Entrevista ObtenerPorId(int idEntrevista)
        {
            return _repository.ObtenerPorId(idEntrevista);
        }

        public List<Oferente> ObtenerOferentes()
        {
            return _repository.ObtenerOferentes();
        }

        public List<Usuario> ObtenerEntrevistadores()
        {
            return _repository.ObtenerEntrevistadores();
        }

        public string Registrar(Entrevista entrevista)
        {
            string validacion = ValidarEntrevista(entrevista, true);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            _repository.Insertar(entrevista);

            return "La entrevista ha sido agendada correctamente.";
        }

        public string Actualizar(Entrevista entrevista)
        {
            string validacion = ValidarEntrevista(entrevista, false);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (entrevista.IdEntrevista <= 0)
                return "Debe seleccionar una entrevista para actualizar.";

            _repository.Actualizar(entrevista);

            return "La entrevista ha sido actualizada correctamente.";
        }

        public string Eliminar(int idEntrevista)
        {
            if (idEntrevista <= 0)
                return "Debe seleccionar una entrevista para eliminar.";

            _repository.Eliminar(idEntrevista);

            return "La entrevista ha sido eliminada correctamente.";
        }

        public string MarcarComoRealizada(int idEntrevista)
        {
            if (idEntrevista <= 0)
                return "Debe seleccionar una entrevista.";

            _repository.MarcarComoRealizada(idEntrevista);

            return "La entrevista ha sido marcada como realizada.";
        }

        private string ValidarEntrevista(Entrevista entrevista, bool validarOferente)
        {
            if (entrevista == null)
                return "Debe ingresar los datos de la entrevista.";

            if (validarOferente && string.IsNullOrWhiteSpace(entrevista.IdentificacionOferente))
                return "Debe seleccionar un oferente.";

            if (entrevista.IdUsuarioEntrevistador <= 0)
                return "Debe seleccionar el empleado que realizará la entrevista.";

            if (entrevista.FechaEntrevista == DateTime.MinValue)
                return "Debe indicar la fecha de la entrevista.";

            return "";
        }
    }
}