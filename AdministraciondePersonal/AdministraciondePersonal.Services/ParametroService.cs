using AdministraciondePersonal.Entities;
using AdministraciondePersonal.Repository;

namespace AdministraciondePersonal.Services
{
    public class ParametroService
    {
        private readonly ParametroRepository _repo;

        public ParametroService(ParametroRepository repo)
        {
            _repo = repo;
        }

        public List<Parametros> ObtenerParametros()
        {
            return _repo.ObtenerParametros();
        }

        public Parametros ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public (bool success, string mensaje) GuardarParametro(
            int id,
            string codigo,
            string valor)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return (
                    false,
                    "El código es obligatorio."
                );

            if (string.IsNullOrWhiteSpace(valor))
                return (
                    false,
                    "El valor es obligatorio."
                );

            if (codigo.Trim().Length > 100)
                return (
                    false,
                    "El código no puede superar los 100 caracteres."
                );

            if (valor.Trim().Length > 500)
                return (
                    false,
                    "El valor no puede superar los 500 caracteres."
                );

            if (id == 0)
            {
                if (_repo.ExisteCodigo(codigo.Trim()))
                {
                    return (
                        false,
                        "Ya existe un parámetro con ese código."
                    );
                }

                _repo.CrearParametro(
                    codigo.Trim(),
                    valor.Trim());

                return (
                    true,
                    "Parámetro creado correctamente."
                );
            }

            _repo.EditarParametro(
                id,
                codigo.Trim(),
                valor.Trim());

            return (
                true,
                "Parámetro actualizado correctamente."
            );
        }

        public (bool success, string mensaje)
            EliminarParametro(int id)
        {
            _repo.EliminarParametro(id);

            return (
                true,
                "Parámetro eliminado correctamente."
            );
        }

        public string ObtenerValor(string codigo)
        {
            return _repo.ObtenerValor(codigo);
        }
    }
}