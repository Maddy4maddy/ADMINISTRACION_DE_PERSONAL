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

        public List<rol> ObtenerRoles()
        {
            return _repo.ObtenerRoles();
        }

        public List<Pantalla> ObtenerPantallas()
        {
            return _repo.ObtenerPantallas();
        }

        public void GuardarRol(int? idRol, string nombreRol, List<Pantalla> pantallas)
        {
            int rolId;

            if (idRol == null || idRol == 0)
            {
                rolId = _repo.CrearRol(nombreRol);
            }
            else
            {
                rolId = idRol.Value;
                _repo.ActualizarRol(rolId, nombreRol);
                _repo.EliminarPantallasRol(rolId);
            }

            foreach (var p in pantallas)
            {
                if (p.Seleccionada)
                {
                    _repo.AsignarPantalla(rolId, p.IdPantalla);
                }
            }
        }

        public void EliminarRol(int idRol)
        {
            _repo.EliminarPantallasRol(idRol);
            _repo.EliminarRol(idRol);
        }
    }
}