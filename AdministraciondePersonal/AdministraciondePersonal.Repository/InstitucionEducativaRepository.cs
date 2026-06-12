using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class InstitucionEducativaRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public InstitucionEducativaRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<InstitucionEducativa> ObtenerTodos()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_institucion AS IdInstitucion,
                    nombre_institucion AS NombreInstitucion
                FROM instituciones_educativas
                ORDER BY id_institucion DESC;";

            return conn.Query<InstitucionEducativa>(sql).ToList();
        }

        public InstitucionEducativa ObtenerPorId(int idInstitucion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_institucion AS IdInstitucion,
                    nombre_institucion AS NombreInstitucion
                FROM instituciones_educativas
                WHERE id_institucion = @IdInstitucion;";

            return conn.QueryFirstOrDefault<InstitucionEducativa>(sql, new
            {
                IdInstitucion = idInstitucion
            });
        }

        public void Insertar(InstitucionEducativa institucion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO instituciones_educativas
                (
                    nombre_institucion
                )
                VALUES
                (
                    @NombreInstitucion
                );";

            conn.Execute(sql, institucion);
        }

        public void Actualizar(InstitucionEducativa institucion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE instituciones_educativas
                SET nombre_institucion = @NombreInstitucion
                WHERE id_institucion = @IdInstitucion;";

            conn.Execute(sql, institucion);
        }

        public bool TieneDatosRelacionados(int idInstitucion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM preparacion_academica
                WHERE id_institucion = @IdInstitucion;";

            int total = conn.ExecuteScalar<int>(sql, new
            {
                IdInstitucion = idInstitucion
            });

            return total > 0;
        }

        public void Eliminar(int idInstitucion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM instituciones_educativas
                WHERE id_institucion = @IdInstitucion;";

            conn.Execute(sql, new
            {
                IdInstitucion = idInstitucion
            });
        }
    }
}