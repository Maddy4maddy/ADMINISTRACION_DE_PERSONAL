using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;
using System.Text.RegularExpressions;

namespace AdministraciondePersonal.Services
{
    public class PreparacionAcademicaService
    {
        private readonly PreparacionAcademicaRepository _repository;

        public PreparacionAcademicaService(PreparacionAcademicaRepository repository)
        {
            _repository = repository;
        }

        public List<PreparacionAcademica> ObtenerPorOferente(string identificacionOferente)
        {
            if (string.IsNullOrWhiteSpace(identificacionOferente))
                return new List<PreparacionAcademica>();

            return _repository.ObtenerPorOferente(identificacionOferente);
        }

        public PreparacionAcademica ObtenerPorId(int idPreparacion)
        {
            return _repository.ObtenerPorId(idPreparacion);
        }

        public List<InstitucionEducativa> ObtenerInstituciones()
        {
            return _repository.ObtenerInstituciones();
        }

        public List<Oferente> ObtenerOferentes()
        {
            return _repository.ObtenerOferentes();
        }

        public string Registrar(PreparacionAcademica preparacion)
        {
            string validacion = ValidarPreparacion(preparacion);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            _repository.Insertar(preparacion);

            return "La preparación académica ha sido registrada correctamente.";
        }

        public string Actualizar(PreparacionAcademica preparacion)
        {
            string validacion = ValidarPreparacion(preparacion);

            if (!string.IsNullOrWhiteSpace(validacion))
                return validacion;

            if (preparacion.IdPreparacion <= 0)
                return "Debe seleccionar una preparación académica para actualizar.";

            _repository.Actualizar(preparacion);

            return "La preparación académica ha sido actualizada correctamente.";
        }

        public string Eliminar(int idPreparacion)
        {
            if (idPreparacion <= 0)
                return "Debe seleccionar una preparación académica para eliminar.";

            if (_repository.TieneDatosRelacionados(idPreparacion))
                return "No se puede eliminar un registro con datos relacionados.";

            _repository.Eliminar(idPreparacion);

            return "La preparación académica ha sido eliminada correctamente.";
        }

        private string ValidarPreparacion(PreparacionAcademica preparacion)
        {
            if (preparacion == null)
                return "Debe ingresar los datos de la preparación académica.";

            if (string.IsNullOrWhiteSpace(preparacion.IdentificacionOferente))
                return "Debe seleccionar un oferente.";

            if (preparacion.IdInstitucion <= 0)
                return "Debe seleccionar una institución educativa.";

            if (string.IsNullOrWhiteSpace(preparacion.TituloObtenido))
                return "El título obtenido es requerido.";

            if (preparacion.TituloObtenido.Length > 100)
                return "El título obtenido debe tener un máximo de 100 caracteres.";

            if (!Regex.IsMatch(preparacion.TituloObtenido, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
                return "El título obtenido solo debe contener letras y espacios.";

            if (preparacion.FechaInicio == DateTime.MinValue)
                return "La fecha de inicio es requerida.";

            if (preparacion.FechaFin == DateTime.MinValue)
                return "La fecha de fin es requerida.";

            if (preparacion.FechaFin < preparacion.FechaInicio)
                return "La fecha de fin debe ser mayor o igual a la fecha de inicio.";

            return "";
        }
    }
}