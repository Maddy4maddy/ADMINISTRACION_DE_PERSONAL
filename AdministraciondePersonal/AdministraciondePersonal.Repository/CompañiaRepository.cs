using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class CompaniaRepository
    {
        private readonly string _connectionString;

        public CompaniaRepository(IConfiguration config)
        {
            _connectionString =
                config.GetConnectionString("DefaultConnection");
        }

        public List<Compania> ObtenerCompanias()
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT
                    id_compania AS IdCompania,
                    nombre_compania AS NombreCompania
                FROM companias
                ORDER BY nombre_compania";

            return conn.Query<Compania>(sql).ToList();
        }

        public void CrearCompania(string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                INSERT INTO companias(nombre_compania)
                VALUES(@NombreCompania)";

            conn.Execute(sql,
                new
                {
                    NombreCompania = nombre
                });
        }

        public void EditarCompania(int id, string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                UPDATE companias
                SET nombre_compania = @NombreCompania
                WHERE id_compania = @IdCompania";

            conn.Execute(sql,
                new
                {
                    IdCompania = id,
                    NombreCompania = nombre
                });
        }

        public void EliminarCompania(int id)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                DELETE FROM companias
                WHERE id_compania = @IdCompania";

            conn.Execute(sql,
                new
                {
                    IdCompania = id
                });
        }

        public bool ExisteCompania(string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT COUNT(*)
                FROM companias
                WHERE UPPER(nombre_compania) =
                      UPPER(@NombreCompania)";

            return conn.ExecuteScalar<int>(
                sql,
                new
                {
                    NombreCompania = nombre
                }) > 0;
        }

        public bool ExisteCompaniaEditar(
            int idCompania,
            string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT COUNT(*)
                FROM companias
                WHERE UPPER(nombre_compania) =
                      UPPER(@NombreCompania)
                AND id_compania <> @IdCompania";

            return conn.ExecuteScalar<int>(
                sql,
                new
                {
                    IdCompania = idCompania,
                    NombreCompania = nombre
                }) > 0;
        }

        public bool CompaniaTieneDatosRelacionados(int id)
        {
            using var conn = new MySqlConnection(_connectionString);

            return false;
        }
    }
}