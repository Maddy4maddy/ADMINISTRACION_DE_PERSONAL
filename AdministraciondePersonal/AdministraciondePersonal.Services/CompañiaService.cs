using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class CompaniaService
    {
        private readonly CompaniaRepository _repo;

        public CompaniaService(CompaniaRepository repo)
        {
            _repo = repo;
        }

        public List<Compania> ObtenerCompanias()
        {
            return _repo.ObtenerCompanias();
        }

        public string CrearCompania(string nombre)
        {
            nombre = nombre?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre de la compañía es obligatorio.";
            }

            if (nombre.Length > 150)
            {
                return "El nombre no puede superar los 150 caracteres.";
            }

            if (_repo.ExisteCompania(nombre))
            {
                return "Ya existe una compañía con ese nombre.";
            }

            _repo.CrearCompania(nombre);

            return "OK";
        }

        public string EditarCompania(int id, string nombre)
        {
            nombre = nombre?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre de la compañía es obligatorio.";
            }

            if (nombre.Length > 150)
            {
                return "El nombre no puede superar los 150 caracteres.";
            }

            if (_repo.ExisteCompaniaEditar(id, nombre))
            {
                return "Ya existe una compañía con ese nombre.";
            }

            _repo.EditarCompania(id, nombre);

            return "OK";
        }

        public string EliminarCompania(int id)
        {
            if (_repo.CompaniaTieneDatosRelacionados(id))
            {
                return "No se puede eliminar porque tiene registros relacionados.";
            }

            _repo.EliminarCompania(id);

            return "OK";
        }
    }
}