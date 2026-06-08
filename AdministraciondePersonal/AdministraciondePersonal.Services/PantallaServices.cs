using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class PantallaService
    {
        private readonly PantallaRepository _repo;

        public Pantalla ObtenerPorRuta(string ruta)
        {
            return _repo.ObtenerPorRuta(ruta);
        }

        public PantallaService(PantallaRepository repo)
        {
            _repo = repo;
        }

        public List<Pantalla> ObtenerPantallas()
        {
            return _repo.ObtenerPantallas();
        }

        public Pantalla ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void GuardarPantalla(
            int id,
            string nombrePantalla,
            string ruta)
        {
            if (id == 0)
            {
                _repo.CrearPantalla(
                    nombrePantalla,
                    ruta);
            }
            else
            {
                _repo.EditarPantalla(
                    id,
                    nombrePantalla,
                    ruta);
            }
        }

        public bool TieneRolesAsignados(int id)
        {
            return _repo.TieneRolesAsignados(id);
        }

        public void EliminarPantalla(int id)
        {
            _repo.EliminarPantalla(id);
        }
    }
}
