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

        public string ObtenerValor(string codigo)
        {
            return _repo.ObtenerValor(codigo);
        }


        public void GuardarParametro(
            int id,
            string codigo,
            string valor)
        {
            if (id == 0)
            {
                _repo.CrearParametro(codigo, valor);
            }
            else
            {
                _repo.EditarParametro(id, codigo, valor);
            }
        }

        public void EliminarParametro(int id)
        {
            _repo.EliminarParametro(id);
        }
    }
}