using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class OferenteRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OferenteRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Oferente> ObtenerTodos()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    o.identificacion AS Identificacion,
                    o.tipo_identificacion AS TipoIdentificacion,
                    o.nombre_completo AS NombreCompleto,
                    o.fecha_nacimiento AS FechaNacimiento,
                    o.correo AS Correo,
                    o.telefono AS Telefono,
                    c.codigo_concurso AS CodigoConcurso,
                    c.nombre_concurso AS NombreConcurso
                FROM oferentes o
                LEFT JOIN oferente_concurso oc
                    ON o.identificacion = oc.identificacion_oferente
                LEFT JOIN concursos c
                    ON oc.codigo_concurso = c.codigo_concurso
                ORDER BY o.nombre_completo;";

            return conn.Query<Oferente>(sql).ToList();
        }

        public Oferente ObtenerPorIdentificacion(string identificacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    o.identificacion AS Identificacion,
                    o.tipo_identificacion AS TipoIdentificacion,
                    o.nombre_completo AS NombreCompleto,
                    o.fecha_nacimiento AS FechaNacimiento,
                    o.correo AS Correo,
                    o.telefono AS Telefono,
                    c.codigo_concurso AS CodigoConcurso,
                    c.nombre_concurso AS NombreConcurso
                FROM oferentes o
                LEFT JOIN oferente_concurso oc
                    ON o.identificacion = oc.identificacion_oferente
                LEFT JOIN concursos c
                    ON oc.codigo_concurso = c.codigo_concurso
                WHERE o.identificacion = @Identificacion
                LIMIT 1;";

            return conn.QueryFirstOrDefault<Oferente>(sql, new
            {
                Identificacion = identificacion
            });
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM oferentes
                WHERE identificacion = @Identificacion;";

            int total = conn.ExecuteScalar<int>(sql, new
            {
                Identificacion = identificacion
            });

            return total > 0;
        }

        public void Insertar(Oferente oferente)
        {
            using IDbConnection conn = _dbFactory.GetConnection();
            conn.Open();

            using IDbTransaction transaction = conn.BeginTransaction();

            try
            {
                string sqlOferente = @"
                    INSERT INTO oferentes
                    (
                        identificacion,
                        tipo_identificacion,
                        nombre_completo,
                        fecha_nacimiento,
                        correo,
                        telefono
                    )
                    VALUES
                    (
                        @Identificacion,
                        @TipoIdentificacion,
                        @NombreCompleto,
                        @FechaNacimiento,
                        @Correo,
                        @Telefono
                    );";

                conn.Execute(sqlOferente, oferente, transaction);

                string sqlConcurso = @"
                    INSERT INTO oferente_concurso
                    (
                        identificacion_oferente,
                        codigo_concurso
                    )
                    VALUES
                    (
                        @Identificacion,
                        @CodigoConcurso
                    );";

                conn.Execute(sqlConcurso, new
                {
                    oferente.Identificacion,
                    oferente.CodigoConcurso
                }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Actualizar(Oferente oferente)
        {
            using IDbConnection conn = _dbFactory.GetConnection();
            conn.Open();

            using IDbTransaction transaction = conn.BeginTransaction();

            try
            {
                string sqlOferente = @"
                    UPDATE oferentes
                    SET
                        tipo_identificacion = @TipoIdentificacion,
                        nombre_completo = @NombreCompleto,
                        fecha_nacimiento = @FechaNacimiento,
                        correo = @Correo,
                        telefono = @Telefono
                    WHERE identificacion = @Identificacion;";

                conn.Execute(sqlOferente, oferente, transaction);

                string sqlEliminarConcurso = @"
                    DELETE FROM oferente_concurso
                    WHERE identificacion_oferente = @Identificacion;";

                conn.Execute(sqlEliminarConcurso, new
                {
                    oferente.Identificacion
                }, transaction);

                string sqlInsertarConcurso = @"
                    INSERT INTO oferente_concurso
                    (
                        identificacion_oferente,
                        codigo_concurso
                    )
                    VALUES
                    (
                        @Identificacion,
                        @CodigoConcurso
                    );";

                conn.Execute(sqlInsertarConcurso, new
                {
                    oferente.Identificacion,
                    oferente.CodigoConcurso
                }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool TieneDatosRelacionados(string identificacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                (
                    SELECT COUNT(*)
                    FROM preparacion_academica
                    WHERE identificacion_oferente = @Identificacion
                )
                +
                (
                    SELECT COUNT(*)
                    FROM experiencia_laboral
                    WHERE identificacion_oferente = @Identificacion
                )
                +
                (
                    SELECT COUNT(*)
                    FROM entrevistas
                    WHERE identificacion_oferente = @Identificacion
                ) AS Total;";

            int total = conn.ExecuteScalar<int>(sql, new
            {
                Identificacion = identificacion
            });

            return total > 0;
        }

        public void Eliminar(string identificacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();
            conn.Open();

            using IDbTransaction transaction = conn.BeginTransaction();

            try
            {
                string sqlEliminarConcursos = @"
                    DELETE FROM oferente_concurso
                    WHERE identificacion_oferente = @Identificacion;";

                conn.Execute(sqlEliminarConcursos, new
                {
                    Identificacion = identificacion
                }, transaction);

                string sqlEliminarOferente = @"
                    DELETE FROM oferentes
                    WHERE identificacion = @Identificacion;";

                conn.Execute(sqlEliminarOferente, new
                {
                    Identificacion = identificacion
                }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Concurso> ObtenerConcursos()
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
                ORDER BY nombre_concurso;";

            return conn.Query<Concurso>(sql).ToList();
        }
    }
}