using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class RolService
    {
        private readonly RolRepository _repo;

        public RolService(RolRepository repo)
        {
            _repo = repo;
        }

        public List<Pantalla> ObtenerPantallas()
        {
            return _repo.ObtenerPantallas();
        }

        public List<rol> ObtenerRoles()
        {
            return _repo.ObtenerRoles();
        }

        public int CrearRolConPantallas(string nombreRol, List<Pantalla> pantallas)
        {
            int idRol = _repo.CrearRol(nombreRol);

            foreach (var p in pantallas)
            {
                if (p.Seleccionada)
                {
                    _repo.AsignarPantalla(idRol, p.IdPantalla);
                }
            }

            return idRol;
        }

        public void EliminarRol(int idRol)
        {
            _repo.EliminarRol(idRol);
        }

        public void EditarRol(int idRol, string nombre)
        {
            _repo.EditarRol(idRol, nombre);
        }
    }
}