using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Text.RegularExpressions;

namespace AdministraciondePersonal.Services
{
    public class ExperienciaLaboralService
    {
        private readonly ExperienciaLaboralRepository _repository;

        public ExperienciaLaboralService(ExperienciaLaboralRepository repository)
        {
            _repository = repository;
        }

        public List<ExperienciaLaboral> ObtenerPorOferente(string identificacionOferente)
        {
            if (string.IsNullOrWhiteSpace(identificacionOferente))
                return new List<ExperienciaLaboral>();

            return _repository.ObtenerPorOferente(identificacionOferente);
        }

        public ExperienciaLaboral ObtenerPorId(int idExperiencia)
        {
            return _repository.ObtenerPorId(idExperiencia);
        }

        public string Registrar(ExperienciaLaboral experiencia)
        {
            string validacion = ValidarExperiencia(experiencia);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            _repository.Insertar(experiencia);

            return "La experiencia laboral ha sido registrada correctamente.";
        }

        public string Actualizar(ExperienciaLaboral experiencia)
        {
            string validacion = ValidarExperiencia(experiencia);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (experiencia.IdExperiencia <= 0)
                return "Debe seleccionar una experiencia laboral para actualizar.";

            _repository.Actualizar(experiencia);

            return "La experiencia laboral ha sido actualizada correctamente.";
        }

        public string Eliminar(int idExperiencia)
        {
            if (idExperiencia <= 0)
                return "Debe seleccionar una experiencia laboral para eliminar.";

            if (_repository.TieneDatosRelacionados(idExperiencia))
                return "No se puede eliminar un registro con datos relacionados.";

            _repository.Eliminar(idExperiencia);

            return "La experiencia laboral ha sido eliminada correctamente.";
        }

        private string ValidarExperiencia(ExperienciaLaboral experiencia)
        {
            if (experiencia == null)
                return "Debe ingresar los datos de la experiencia laboral.";

            if (string.IsNullOrWhiteSpace(experiencia.IdentificacionOferente))
                return "Debe seleccionar un oferente.";

            if (string.IsNullOrWhiteSpace(experiencia.NombreEmpresa))
                return "El nombre de la empresa es requerido.";

            if (experiencia.NombreEmpresa.Length > 100)
                return "El nombre de la empresa debe tener un máximo de 100 caracteres.";

            if (!Regex.IsMatch(experiencia.NombreEmpresa, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s]+$"))
                return "El nombre de la empresa solo debe contener letras, números y espacios.";

            if (string.IsNullOrWhiteSpace(experiencia.PuestoDesempenado))
                return "El puesto desempeñado es requerido.";

            if (experiencia.PuestoDesempenado.Length > 100)
                return "El puesto desempeñado debe tener un máximo de 100 caracteres.";

            if (!Regex.IsMatch(experiencia.PuestoDesempenado, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s]+$"))
                return "El puesto desempeñado solo debe contener letras, números y espacios.";

            if (experiencia.FechaInicio == DateTime.MinValue)
                return "La fecha de inicio es requerida.";

            if (experiencia.FechaFin == DateTime.MinValue)
                return "La fecha de fin es requerida.";

            if (experiencia.FechaFin < experiencia.FechaInicio)
                return "La fecha de fin debe ser mayor o igual a la fecha de inicio.";

            return "";
        }
    }
}