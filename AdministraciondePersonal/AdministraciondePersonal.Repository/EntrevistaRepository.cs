using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class EntrevistaRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public EntrevistaRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Entrevista> ObtenerPaginado(int pagina, int tamanioPagina)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            int offset = (pagina - 1) * tamanioPagina;

            string sql = @"
                SELECT 
                    e.id_entrevista AS IdEntrevista,
                    e.identificacion_oferente AS IdentificacionOferente,
                    o.nombre_completo AS NombreOferente,
                    e.id_usuario_entrevistador AS IdUsuarioEntrevistador,
                    u.nombre_completo AS NombreEntrevistador,
                    e.fecha_entrevista AS FechaEntrevista,
                    e.estado AS Estado
                FROM entrevistas e
                INNER JOIN oferentes o 
                    ON e.identificacion_oferente = o.identificacion
                INNER JOIN usuarios u 
                    ON e.id_usuario_entrevistador = u.id_usuario
                ORDER BY e.fecha_entrevista ASC
                LIMIT @TamanioPagina OFFSET @Offset;";

            return conn.Query<Entrevista>(sql, new
            {
                TamanioPagina = tamanioPagina,
                Offset = offset
            }).ToList();
        }

        public int ContarEntrevistas()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = "SELECT COUNT(*) FROM entrevistas;";

            return conn.ExecuteScalar<int>(sql);
        }

        public Entrevista ObtenerPorId(int idEntrevista)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT 
                    e.id_entrevista AS IdEntrevista,
                    e.identificacion_oferente AS IdentificacionOferente,
                    o.nombre_completo AS NombreOferente,
                    e.id_usuario_entrevistador AS IdUsuarioEntrevistador,
                    u.nombre_completo AS NombreEntrevistador,
                    e.fecha_entrevista AS FechaEntrevista,
                    e.estado AS Estado
                FROM entrevistas e
                INNER JOIN oferentes o 
                    ON e.identificacion_oferente = o.identificacion
                INNER JOIN usuarios u 
                    ON e.id_usuario_entrevistador = u.id_usuario
                WHERE e.id_entrevista = @IdEntrevista;";

            return conn.QueryFirstOrDefault<Entrevista>(sql, new
            {
                IdEntrevista = idEntrevista
            });
        }

        public void Insertar(Entrevista entrevista)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO entrevistas
                (
                    identificacion_oferente,
                    id_usuario_entrevistador,
                    fecha_entrevista,
                    estado
                )
                VALUES
                (
                    @IdentificacionOferente,
                    @IdUsuarioEntrevistador,
                    @FechaEntrevista,
                    'Pendiente'
                );";

            conn.Execute(sql, entrevista);
        }

        public void Actualizar(Entrevista entrevista)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE entrevistas
                SET
                    id_usuario_entrevistador = @IdUsuarioEntrevistador,
                    fecha_entrevista = @FechaEntrevista
                WHERE id_entrevista = @IdEntrevista;";

            conn.Execute(sql, entrevista);
        }

        public void Eliminar(int idEntrevista)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM entrevistas
                WHERE id_entrevista = @IdEntrevista;";

            conn.Execute(sql, new
            {
                IdEntrevista = idEntrevista
            });
        }

        public void MarcarComoRealizada(int idEntrevista)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE entrevistas
                SET estado = 'Realizada'
                WHERE id_entrevista = @IdEntrevista;";

            conn.Execute(sql, new
            {
                IdEntrevista = idEntrevista
            });
        }

        public List<Oferente> ObtenerOferentes()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT 
                    identificacion AS Identificacion,
                    nombre_completo AS NombreCompleto
                FROM oferentes
                ORDER BY nombre_completo;";

            return conn.Query<Oferente>(sql).ToList();
        }

        public List<Usuario> ObtenerEntrevistadores()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT 
                    id_usuario AS IdUsuario,
                    nombre_usuario AS NombreUsuario,
                    nombre_completo AS NombreCompleto
                FROM usuarios
                WHERE estado = 'activo'
                ORDER BY nombre_completo;";

            return conn.Query<Usuario>(sql).ToList();
        }
    }
}