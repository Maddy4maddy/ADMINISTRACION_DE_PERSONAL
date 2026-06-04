using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Text.RegularExpressions;

namespace AdministraciondePersonal.Services
{
    public class OferenteService
    {
        private readonly OferenteRepository _repository;

        public OferenteService(OferenteRepository repository)
        {
            _repository = repository;
        }

        public List<Oferente> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public Oferente ObtenerPorIdentificacion(string identificacion)
        {
            return _repository.ObtenerPorIdentificacion(identificacion);
        }

        public List<Concurso> ObtenerConcursos()
        {
            return _repository.ObtenerConcursos();
        }

        public string Registrar(Oferente oferente)
        {
            string validacion = ValidarOferente(oferente);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (_repository.ExisteIdentificacion(oferente.Identificacion))
                return "El número de identificación ya existe.";

            _repository.Insertar(oferente);

            return "El oferente ha sido registrado correctamente.";
        }

        public string Actualizar(Oferente oferente)
        {
            string validacion = ValidarOferente(oferente);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (!_repository.ExisteIdentificacion(oferente.Identificacion))
                return "El oferente seleccionado no existe.";

            _repository.Actualizar(oferente);

            return "El oferente ha sido actualizado correctamente.";
        }

        public string Eliminar(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
                return "Debe seleccionar un oferente para eliminar.";

            if (!_repository.ExisteIdentificacion(identificacion))
                return "El oferente seleccionado no existe.";

            if (_repository.TieneDatosRelacionados(identificacion))
                return "No se puede eliminar un registro con datos relacionados.";

            _repository.Eliminar(identificacion);

            return "El oferente ha sido eliminado correctamente.";
        }

        private string ValidarOferente(Oferente oferente)
        {
            if (oferente == null)
                return "Debe ingresar los datos del oferente.";

            if (string.IsNullOrWhiteSpace(oferente.Identificacion))
                return "La identificación es requerida.";

            if (string.IsNullOrWhiteSpace(oferente.TipoIdentificacion))
                return "El tipo de identificación es requerido.";

            if (string.IsNullOrWhiteSpace(oferente.NombreCompleto))
                return "El nombre completo es requerido.";

            if (oferente.FechaNacimiento == DateTime.MinValue)
                return "La fecha de nacimiento es requerida.";

            if (string.IsNullOrWhiteSpace(oferente.Correo))
                return "El correo es requerido.";

            if (!Regex.IsMatch(oferente.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Correo inválido.";

            if (string.IsNullOrWhiteSpace(oferente.Telefono))
                return "El teléfono es requerido.";

            if (oferente.CodigoConcurso <= 0)
                return "Debe seleccionar un concurso.";

            return "";
        }
    }
}