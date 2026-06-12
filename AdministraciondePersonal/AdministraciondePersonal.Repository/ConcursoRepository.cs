using AdministraciondePersonal.Entities;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class ConcursoRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ConcursoRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Concurso> ObtenerTodos()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    codigo_concurso AS CodigoConcurso,
                    nombre_concurso AS NombreConcurso,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin,
                    estado AS Estado
                FROM concursos
                ORDER BY codigo_concurso DESC;";

            return Dapper.SqlMapper.Query<Concurso>(conn, sql).ToList();
        }

        public Concurso ObtenerPorCodigo(int codigoConcurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    codigo_concurso AS CodigoConcurso,
                    nombre_concurso AS NombreConcurso,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin,
                    estado AS Estado
                FROM concursos
                WHERE codigo_concurso = @CodigoConcurso;";

            return Dapper.SqlMapper.QueryFirstOrDefault<Concurso>(conn, sql, new
            {
                CodigoConcurso = codigoConcurso
            });
        }

        public bool ExisteCodigo(int codigoConcurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM concursos
                WHERE codigo_concurso = @CodigoConcurso;";

            int total = Dapper.SqlMapper.ExecuteScalar<int>(conn, sql, new
            {
                CodigoConcurso = codigoConcurso
            });

            return total > 0;
        }

        public void Insertar(Concurso concurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO concursos
                (
                    codigo_concurso,
                    nombre_concurso,
                    fecha_inicio,
                    fecha_fin,
                    estado
                )
                VALUES
                (
                    @CodigoConcurso,
                    @NombreConcurso,
                    @FechaInicio,
                    @FechaFin,
                    @Estado
                );";

            Dapper.SqlMapper.Execute(conn, sql, concurso);
        }

        public void Actualizar(Concurso concurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE concursos
                SET
                    nombre_concurso = @NombreConcurso,
                    fecha_inicio = @FechaInicio,
                    fecha_fin = @FechaFin,
                    estado = @Estado
                WHERE codigo_concurso = @CodigoConcurso;";

            Dapper.SqlMapper.Execute(conn, sql, concurso);
        }

        public bool TieneDatosRelacionados(int codigoConcurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM oferente_concurso
                WHERE codigo_concurso = @CodigoConcurso;";

            int total = Dapper.SqlMapper.ExecuteScalar<int>(conn, sql, new
            {
                CodigoConcurso = codigoConcurso
            });

            return total > 0;
        }

        public void Eliminar(int codigoConcurso)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM concursos
                WHERE codigo_concurso = @CodigoConcurso;";

            Dapper.SqlMapper.Execute(conn, sql, new
            {
                CodigoConcurso = codigoConcurso
            });
        }

        public void CambiarEstado(int codigoConcurso, string estado)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE concursos
                SET estado = @Estado
                WHERE codigo_concurso = @CodigoConcurso;";

            Dapper.SqlMapper.Execute(conn, sql, new
            {
                CodigoConcurso = codigoConcurso,
                Estado = estado
            });
        }
    }
}