using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class ParametroRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ParametroRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Parametros> ObtenerParametros()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_parametro AS IdParametro,
                    codigo AS Codigo,
                    valor AS Valor
                FROM parametros
                ORDER BY codigo";

            return conn.Query<Parametros>(sql).ToList();
        }

        public Parametros ObtenerPorId(int id)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_parametro AS IdParametro,
                    codigo AS Codigo,
                    valor AS Valor
                FROM parametros
                WHERE id_parametro = @Id";

            return conn.QueryFirstOrDefault<Parametros>(
                sql,
                new { Id = id });
        }

        public bool ExisteCodigo(string codigo)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM parametros
                WHERE UPPER(codigo) = UPPER(@Codigo)";

            return conn.ExecuteScalar<int>(
                sql,
                new { Codigo = codigo }) > 0;
        }

        public string ObtenerValor(string codigo)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
        SELECT valor
        FROM parametros
        WHERE codigo = @Codigo";

            return conn.QueryFirstOrDefault<string>(
                sql,
                new { Codigo = codigo });
        }

        public void CrearParametro(
            string codigo,
            string valor)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO parametros
                (
                    codigo,
                    valor
                )
                VALUES
                (
                    @Codigo,
                    @Valor
                )";

            conn.Execute(sql,
                new
                {
                    Codigo = codigo,
                    Valor = valor
                });
        }

        public void EditarParametro(
            int id,
            string codigo,
            string valor)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE parametros
                SET
                    codigo = @Codigo,
                    valor = @Valor
                WHERE id_parametro = @Id";

            conn.Execute(sql,
                new
                {
                    Id = id,
                    Codigo = codigo,
                    Valor = valor
                });
        }

        public void EliminarParametro(int id)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM parametros
                WHERE id_parametro = @Id";

            conn.Execute(sql,
                new { Id = id });
        }
    }
}