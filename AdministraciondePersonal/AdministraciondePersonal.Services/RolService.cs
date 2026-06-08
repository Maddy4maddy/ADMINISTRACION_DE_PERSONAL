using AdministraciondePersonal.Repository;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Services
{
    public class RolService
    {
        private readonly RolRepository _repo;

        public RolService(RolRepository repo)
        {
            _repo = repo;
        }

        public List<rol> ObtenerRoles()
        {
            return _repo.ObtenerRoles();
        }

        public List<Pantalla> ObtenerPantallas()
        {
            return _repo.ObtenerPantallas();
        }

        public List<int> ObtenerPantallasPorRol(int idRol)
        {
            return _repo.ObtenerPantallasPorRol(idRol);
        }

        public void CrearRolConPantallas(string nombreRol, List<Pantalla> pantallas)
        {
            int idRol = _repo.CrearRol(nombreRol);

            foreach (var p in pantallas)
            {
                if (p.Seleccionada)
                {
                    _repo.AsignarPantalla(idRol, p.IdPantalla);
                }
            }
        }

        public void EditarRol(int idRol, string nombreRol, List<Pantalla> pantallas)
        {
            _repo.EditarRol(idRol, nombreRol);

            _repo.EliminarPantallasPorRol(idRol);

            foreach (var p in pantallas)
            {
                if (p.Seleccionada)
                {
                    _repo.AsignarPantalla(idRol, p.IdPantalla);
                }
            }
        }

        public string EliminarRol(int idRol)
        {
            if (_repo.RolTieneUsuarios(idRol))
            {
                return "No se puede eliminar el rol porque tiene usuarios asignados.";
            }

            _repo.EliminarPantallasPorRol(idRol);
            _repo.EliminarRol(idRol);

            return "OK";
        }
    }
}