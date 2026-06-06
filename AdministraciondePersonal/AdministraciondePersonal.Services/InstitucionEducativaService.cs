using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Text.RegularExpressions;

namespace AdministraciondePersonal.Services
{
    public class InstitucionEducativaService
    {
        private readonly InstitucionEducativaRepository _repository;

        public InstitucionEducativaService(InstitucionEducativaRepository repository)
        {
            _repository = repository;
        }

        public List<InstitucionEducativa> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public InstitucionEducativa ObtenerPorId(int idInstitucion)
        {
            return _repository.ObtenerPorId(idInstitucion);
        }

        public string Registrar(InstitucionEducativa institucion)
        {
            string validacion = ValidarInstitucion(institucion);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            _repository.Insertar(institucion);

            return "La institución educativa ha sido registrada correctamente.";
        }

        public string Actualizar(InstitucionEducativa institucion)
        {
            string validacion = ValidarInstitucion(institucion);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (institucion.IdInstitucion <= 0)
                return "Debe seleccionar una institución educativa para actualizar.";

            _repository.Actualizar(institucion);

            return "La institución educativa ha sido actualizada correctamente.";
        }

        public string Eliminar(int idInstitucion)
        {
            if (idInstitucion <= 0)
                return "Debe seleccionar una institución educativa para eliminar.";

            if (_repository.TieneDatosRelacionados(idInstitucion))
                return "No se puede eliminar un registro con datos relacionados.";

            _repository.Eliminar(idInstitucion);

            return "La institución educativa ha sido eliminada correctamente.";
        }

        private string ValidarInstitucion(InstitucionEducativa institucion)
        {
            if (institucion == null)
                return "Debe ingresar los datos de la institución educativa.";

            if (string.IsNullOrWhiteSpace(institucion.NombreInstitucion))
                return "El nombre de la institución es requerido.";

            if (institucion.NombreInstitucion.Length > 150)
                return "El nombre de la institución debe tener un máximo de 150 caracteres.";

            if (!Regex.IsMatch(institucion.NombreInstitucion, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
                return "El nombre de la institución solo debe contener letras y espacios.";

            return "";
        }
    }
}