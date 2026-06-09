using System.Collections.Generic;
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

        public void CrearCompania(string nombre)
        {
            _repo.CrearCompania(nombre);
        }

        public void EditarCompania(int id, string nombre)
        {
            _repo.EditarCompania(id, nombre);
        }

        public string EliminarCompania(int id)
        {
            if (_repo.CompaniaTieneDatosRelacionados(id))
            {
                return "No se puede eliminar un registro con datos relacionados.";
            }

            _repo.EliminarCompania(id);

            return "OK";
        }
    }
}