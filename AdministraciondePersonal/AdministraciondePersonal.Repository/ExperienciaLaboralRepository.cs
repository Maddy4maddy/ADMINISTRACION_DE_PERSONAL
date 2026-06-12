using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class ExperienciaLaboralRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ExperienciaLaboralRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<ExperienciaLaboral> ObtenerPorOferente(string identificacionOferente)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_experiencia AS IdExperiencia,
                    identificacion_oferente AS IdentificacionOferente,
                    nombre_empresa AS NombreEmpresa,
                    puesto_desempenado AS PuestoDesempenado,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin
                FROM experiencia_laboral
                WHERE identificacion_oferente = @IdentificacionOferente
                ORDER BY id_experiencia DESC;";

            return conn.Query<ExperienciaLaboral>(sql, new
            {
                IdentificacionOferente = identificacionOferente
            }).ToList();
        }

        public ExperienciaLaboral ObtenerPorId(int idExperiencia)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_experiencia AS IdExperiencia,
                    identificacion_oferente AS IdentificacionOferente,
                    nombre_empresa AS NombreEmpresa,
                    puesto_desempenado AS PuestoDesempenado,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin
                FROM experiencia_laboral
                WHERE id_experiencia = @IdExperiencia;";

            return conn.QueryFirstOrDefault<ExperienciaLaboral>(sql, new
            {
                IdExperiencia = idExperiencia
            });
        }

        public void Insertar(ExperienciaLaboral experiencia)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO experiencia_laboral
                (
                    identificacion_oferente,
                    nombre_empresa,
                    puesto_desempenado,
                    fecha_inicio,
                    fecha_fin
                )
                VALUES
                (
                    @IdentificacionOferente,
                    @NombreEmpresa,
                    @PuestoDesempenado,
                    @FechaInicio,
                    @FechaFin
                );";

            conn.Execute(sql, experiencia);
        }

        public void Actualizar(ExperienciaLaboral experiencia)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE experiencia_laboral
                SET
                    identificacion_oferente = @IdentificacionOferente,
                    nombre_empresa = @NombreEmpresa,
                    puesto_desempenado = @PuestoDesempenado,
                    fecha_inicio = @FechaInicio,
                    fecha_fin = @FechaFin
                WHERE id_experiencia = @IdExperiencia;";

            conn.Execute(sql, experiencia);
        }

        public bool TieneDatosRelacionados(int idExperiencia)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM experiencia_laboral_asignacion
                WHERE id_experiencia = @IdExperiencia;";

            int total = conn.ExecuteScalar<int>(sql, new
            {
                IdExperiencia = idExperiencia
            });

            return total > 0;
        }

        public void Eliminar(int idExperiencia)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM experiencia_laboral
                WHERE id_experiencia = @IdExperiencia;";

            conn.Execute(sql, new
            {
                IdExperiencia = idExperiencia
            });
        }
    }
}