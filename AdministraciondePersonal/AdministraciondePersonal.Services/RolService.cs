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

        public List<int> ObtenerPantallasPorRol(int idRol)
        {
            return _repo.ObtenerPantallasPorRol(idRol);
        }

        public void CrearRolConPantallas(
            string nombreRol,
            List<Pantalla> pantallas)
        {
            ValidarNombreRol(nombreRol);

            if (_repo.ExisteRol(nombreRol))
            {
                throw new Exception(
                    "Ya existe un rol con ese nombre.");
            }

            int idRol =
                _repo.CrearRol(nombreRol.Trim());

            foreach (var pantalla in pantallas)
            {
                if (pantalla.Seleccionada)
                {
                    _repo.AsignarPantalla(
                        idRol,
                        pantalla.IdPantalla);
                }
            }
        }

        public void EditarRol(
            int idRol,
            string nombreRol,
            List<Pantalla> pantallas)
        {
            ValidarNombreRol(nombreRol);

            _repo.EditarRol(
                idRol,
                nombreRol.Trim());

            _repo.EliminarPantallasPorRol(idRol);

            foreach (var pantalla in pantallas)
            {
                if (pantalla.Seleccionada)
                {
                    _repo.AsignarPantalla(
                        idRol,
                        pantalla.IdPantalla);
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

        private void ValidarNombreRol(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                throw new Exception(
                    "El nombre del rol es obligatorio.");
            }

            if (nombreRol.Trim().Length > 40)
            {
                throw new Exception(
                    "El nombre del rol no puede superar los 40 caracteres.");
            }
        }
    }
}