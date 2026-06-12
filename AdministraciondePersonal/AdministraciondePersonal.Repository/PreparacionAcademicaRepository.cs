using AdministraciondePersonal.Entities;
using Dapper;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class PreparacionAcademicaRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public PreparacionAcademicaRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<PreparacionAcademica> ObtenerPorOferente(string identificacionOferente)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    pa.id_preparacion AS IdPreparacion,
                    pa.identificacion_oferente AS IdentificacionOferente,
                    pa.id_institucion AS IdInstitucion,
                    ie.nombre_institucion AS NombreInstitucion,
                    pa.titulo_obtenido AS TituloObtenido,
                    pa.fecha_inicio AS FechaInicio,
                    pa.fecha_fin AS FechaFin
                FROM preparacion_academica pa
                INNER JOIN instituciones_educativas ie
                    ON pa.id_institucion = ie.id_institucion
                WHERE pa.identificacion_oferente = @IdentificacionOferente
                ORDER BY pa.id_preparacion DESC;";

            return conn.Query<PreparacionAcademica>(sql, new
            {
                IdentificacionOferente = identificacionOferente
            }).ToList();
        }

        public PreparacionAcademica ObtenerPorId(int idPreparacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    pa.id_preparacion AS IdPreparacion,
                    pa.identificacion_oferente AS IdentificacionOferente,
                    pa.id_institucion AS IdInstitucion,
                    ie.nombre_institucion AS NombreInstitucion,
                    pa.titulo_obtenido AS TituloObtenido,
                    pa.fecha_inicio AS FechaInicio,
                    pa.fecha_fin AS FechaFin
                FROM preparacion_academica pa
                INNER JOIN instituciones_educativas ie
                    ON pa.id_institucion = ie.id_institucion
                WHERE pa.id_preparacion = @IdPreparacion;";

            return conn.QueryFirstOrDefault<PreparacionAcademica>(sql, new
            {
                IdPreparacion = idPreparacion
            });
        }

        public void Insertar(PreparacionAcademica preparacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                INSERT INTO preparacion_academica
                (
                    identificacion_oferente,
                    id_institucion,
                    titulo_obtenido,
                    fecha_inicio,
                    fecha_fin
                )
                VALUES
                (
                    @IdentificacionOferente,
                    @IdInstitucion,
                    @TituloObtenido,
                    @FechaInicio,
                    @FechaFin
                );";

            conn.Execute(sql, preparacion);
        }

        public void Actualizar(PreparacionAcademica preparacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                UPDATE preparacion_academica
                SET
                    identificacion_oferente = @IdentificacionOferente,
                    id_institucion = @IdInstitucion,
                    titulo_obtenido = @TituloObtenido,
                    fecha_inicio = @FechaInicio,
                    fecha_fin = @FechaFin
                WHERE id_preparacion = @IdPreparacion;";

            conn.Execute(sql, preparacion);
        }

        public bool TieneDatosRelacionados(int idPreparacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT COUNT(*)
                FROM preparacion_academica_asignacion
                WHERE id_preparacion = @IdPreparacion;";

            int total = conn.ExecuteScalar<int>(sql, new
            {
                IdPreparacion = idPreparacion
            });

            return total > 0;
        }

        public void Eliminar(int idPreparacion)
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                DELETE FROM preparacion_academica
                WHERE id_preparacion = @IdPreparacion;";

            conn.Execute(sql, new
            {
                IdPreparacion = idPreparacion
            });
        }

        public List<InstitucionEducativa> ObtenerInstituciones()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    id_institucion AS IdInstitucion,
                    nombre_institucion AS NombreInstitucion
                FROM instituciones_educativas
                ORDER BY nombre_institucion;";

            return conn.Query<InstitucionEducativa>(sql).ToList();
        }

        public List<Oferente> ObtenerOferentes()
        {
            using IDbConnection conn = _dbFactory.GetConnection();

            string sql = @"
                SELECT
                    identificacion AS Identificacion,
                    tipo_identificacion AS TipoIdentificacion,
                    nombre_completo AS NombreCompleto,
                    fecha_nacimiento AS FechaNacimiento,
                    correo AS Correo,
                    telefono AS Telefono
                FROM oferentes
                ORDER BY nombre_completo;";

            return conn.Query<Oferente>(sql).ToList();
        }
    }
}