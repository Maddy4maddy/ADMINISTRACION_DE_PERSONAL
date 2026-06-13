using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class RolRepository
    {
        private readonly string _connectionString;

        public RolRepository(IConfiguration config)
        {
            _connectionString =
                config.GetConnectionString("DefaultConnection");
        }

        public List<rol> ObtenerRoles()
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT
                    id_rol AS IdRol,
                    nombre_rol AS NombreRol
                FROM roles
                ORDER BY nombre_rol";

            return conn.Query<rol>(sql).ToList();
        }

        public List<Pantalla> ObtenerPantallas()
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT
                    id_pantalla AS IdPantalla,
                    nombre_pantalla AS NombrePantalla,
                    ruta AS Ruta
                FROM pantallas";

            return conn.Query<Pantalla>(sql).ToList();
        }

        public int CrearRol(string nombreRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                INSERT INTO roles(nombre_rol)
                VALUES(@NombreRol);

                SELECT LAST_INSERT_ID();";

            return conn.ExecuteScalar<int>(
                sql,
                new { NombreRol = nombreRol });
        }

        public void EditarRol(int idRol, string nombreRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                UPDATE roles
                SET nombre_rol = @NombreRol
                WHERE id_rol = @IdRol";

            conn.Execute(sql,
                new
                {
                    IdRol = idRol,
                    NombreRol = nombreRol
                });
        }

        public void EliminarRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                DELETE FROM roles
                WHERE id_rol = @IdRol";

            conn.Execute(sql,
                new { IdRol = idRol });
        }

        public void EliminarPantallasPorRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                DELETE FROM RolPantalla
                WHERE id_rol = @IdRol";

            conn.Execute(sql,
                new { IdRol = idRol });
        }

        public void AsignarPantalla(int idRol, int idPantalla)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                INSERT INTO RolPantalla
                (
                    id_rol,
                    id_pantalla
                )
                VALUES
                (
                    @IdRol,
                    @IdPantalla
                )";

            conn.Execute(sql,
                new
                {
                    IdRol = idRol,
                    IdPantalla = idPantalla
                });
        }

        public List<int> ObtenerPantallasPorRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT id_pantalla
                FROM RolPantalla
                WHERE id_rol = @IdRol";

            return conn.Query<int>(
                sql,
                new { IdRol = idRol })
                .ToList();
        }

        public bool RolTieneUsuarios(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT COUNT(*)
                FROM usuarios
                WHERE id_rol = @IdRol";

            return conn.ExecuteScalar<int>(
                sql,
                new { IdRol = idRol }) > 0;
        }

        public bool ExisteRol(string nombreRol)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT COUNT(*)
                FROM roles
                WHERE UPPER(nombre_rol) =
                      UPPER(@NombreRol)";

            return conn.ExecuteScalar<int>(
                sql,
                new { NombreRol = nombreRol }) > 0;
        }
    }
}