using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class BitacoraService
    {
        private readonly BitacoraRepository _bitacoraRepository;

        public BitacoraService(BitacoraRepository bitacoraRepository)
        {
            _bitacoraRepository = bitacoraRepository;
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