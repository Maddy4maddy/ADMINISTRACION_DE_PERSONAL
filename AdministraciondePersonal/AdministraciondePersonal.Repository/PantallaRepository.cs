using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class PantallaRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public PantallaRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Pantalla> ObtenerPantallas()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_pantalla AS IdPantalla,
                    nombre_pantalla AS NombrePantalla,
                    ruta AS Ruta
                FROM pantallas
                ORDER BY nombre_pantalla";

            return conn.Query<Pantalla>(sql).ToList();
        }

        public Pantalla ObtenerPorId(int id)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_pantalla AS IdPantalla,
                    nombre_pantalla AS NombrePantalla,
                    ruta AS Ruta
                FROM pantallas
                WHERE id_pantalla = @Id";

            return conn.QueryFirstOrDefault<Pantalla>(
                sql,
                new { Id = id });
        }

        public Pantalla ObtenerPorRuta(string ruta)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_pantalla AS IdPantalla,
                    nombre_pantalla AS NombrePantalla,
                    ruta AS Ruta
                FROM pantallas
                WHERE ruta = @Ruta";

            return conn.QueryFirstOrDefault<Pantalla>(
                sql,
                new { Ruta = ruta });
        }

        public bool ExisteNombrePantalla(string nombrePantalla)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM pantallas
                WHERE UPPER(nombre_pantalla) =
                      UPPER(@NombrePantalla)";

            return conn.ExecuteScalar<int>(
                sql,
                new { NombrePantalla = nombrePantalla }) > 0;
        }

        public bool ExisteRuta(string ruta)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM pantallas
                WHERE UPPER(ruta) =
                      UPPER(@Ruta)";

            return conn.ExecuteScalar<int>(
                sql,
                new { Ruta = ruta }) > 0;
        }

        public void CrearPantalla(
            string nombrePantalla,
            string ruta)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO pantallas
                (
                    nombre_pantalla,
                    ruta
                )
                VALUES
                (
                    @NombrePantalla,
                    @Ruta
                )";

            conn.Execute(sql,
                new
                {
                    NombrePantalla = nombrePantalla,
                    Ruta = ruta
                });
        }

        public void EditarPantalla(
            int id,
            string nombrePantalla,
            string ruta)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE pantallas
                SET
                    nombre_pantalla = @NombrePantalla,
                    ruta = @Ruta
                WHERE id_pantalla = @Id";

            conn.Execute(sql,
                new
                {
                    Id = id,
                    NombrePantalla = nombrePantalla,
                    Ruta = ruta
                });
        }

        public bool TieneRolesAsignados(int idPantalla)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM rolpantalla
                WHERE id_pantalla = @IdPantalla";

            return conn.ExecuteScalar<int>(
                sql,
                new { IdPantalla = idPantalla }) > 0;
        }

        public void EliminarPantalla(int idPantalla)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM pantallas
                WHERE id_pantalla = @IdPantalla";

            conn.Execute(sql,
                new { IdPantalla = idPantalla });
        }
        public List<Pantalla> ObtenerPantallasPorRoles(List<int> rolesIds)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
        SELECT DISTINCT
            p.id_pantalla AS IdPantalla,
            p.nombre_pantalla AS NombrePantalla,
            p.ruta AS Ruta
        FROM pantallas p
        INNER JOIN rolpantalla rp
            ON p.id_pantalla = rp.id_pantalla
        WHERE rp.id_rol IN @RolesIds
        ORDER BY p.id_pantalla";

            return conn.Query<Pantalla>(
                sql,
                new { RolesIds = rolesIds }
            ).ToList();
        }
    }
}