using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class PantallaService
    {
        private readonly PantallaRepository _repo;

        public PantallaService(PantallaRepository repo)
        {
            _repo = repo;
        }
        public List<Pantalla> ObtenerPantallasPorRoles(List<int> rolesIds)
        {
            return _repo.ObtenerPantallasPorRoles(rolesIds);
        }
        public List<Pantalla> ObtenerPantallas()
        {
            return _repo.ObtenerPantallas();
        }

        public Pantalla ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public Pantalla ObtenerPorRuta(string ruta)
        {
            return _repo.ObtenerPorRuta(ruta);
        }

        public (bool success, string mensaje) GuardarPantalla(
            int id,
            string nombrePantalla,
            string ruta)
        {
            if (string.IsNullOrWhiteSpace(nombrePantalla))
                return (false,
                    "El nombre de la pantalla es obligatorio.");

            if (string.IsNullOrWhiteSpace(ruta))
                return (false,
                    "La ruta es obligatoria.");

            if (nombrePantalla.Trim().Length > 100)
                return (false,
                    "El nombre de la pantalla no puede superar los 100 caracteres.");

            if (ruta.Trim().Length > 100)
                return (false,
                    "La ruta no puede superar los 100 caracteres.");

            if (id == 0)
            {
                if (_repo.ExisteNombrePantalla(nombrePantalla.Trim()))
                    return (false,
                        "Ya existe una pantalla con ese nombre.");

                if (_repo.ExisteRuta(ruta.Trim()))
                    return (false,
                        "Ya existe una pantalla con esa ruta.");

                _repo.CrearPantalla(
                    nombrePantalla.Trim(),
                    ruta.Trim());

                return (true,
                    "Pantalla creada correctamente.");
            }

            _repo.EditarPantalla(
                id,
                nombrePantalla.Trim(),
                ruta.Trim());

            return (true,
                "Pantalla actualizada correctamente.");
        }

        public (bool success, string mensaje)
            EliminarPantalla(int id)
        {
            if (_repo.TieneRolesAsignados(id))
            {
                return (
                    false,
                    "No se puede eliminar la pantalla porque tiene roles asignados."
                );
            }

            _repo.EliminarPantalla(id);

            return (
                true,
                "Pantalla eliminada correctamente."
            );
        }
    }
}