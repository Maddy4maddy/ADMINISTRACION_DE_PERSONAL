using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class BitacoraService
    {
        // Implementación Singleton
        private static BitacoraService _instancia;
        private static readonly object _lock = new object();
        private readonly BitacoraRepository _bitacoraRepository;

        private BitacoraService(BitacoraRepository bitacoraRepository)
        {
            _bitacoraRepository = bitacoraRepository;
        }

        public static BitacoraService GetInstance(BitacoraRepository bitacoraRepository)
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new BitacoraService(bitacoraRepository);
                    }
                }
            }
            return _instancia;
        }

        // MICROSERVICIO BITACORA
        public void RegistrarAccion(string usuario, string descripcionAccion)
        {
            if (string.IsNullOrEmpty(usuario))
                throw new ArgumentException("El usuario es requerido");

            if (string.IsNullOrEmpty(descripcionAccion))
                throw new ArgumentException("La descripción de la acción es requerida");

            _bitacoraRepository.Registrar(usuario, descripcionAccion);
        }

        public (List<Bitacora> items, int totalCount) ObtenerBitacoras(
            int pageIndex = 1,
            int pageSize = 100,
            string ordenarPor = "FechaBitacora",
            string direccion = "DESC",
            string filtroUsuario = null,
            string filtroDescripcion = null)
        {
            return _bitacoraRepository.ObtenerBitacoras(pageIndex, pageSize, ordenarPor, direccion, filtroUsuario, filtroDescripcion);
        }

        public List<string> ObtenerUsuariosUnicos()
        {
            return _bitacoraRepository.ObtenerUsuariosUnicos();
        }
    }
}