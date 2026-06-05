using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
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
            List<ExperienciaLaboral> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    id_experiencia,
                    identificacion_oferente,
                    nombre_empresa,
                    puesto_desempenado,
                    fecha_inicio,
                    fecha_fin
                FROM experiencia_laboral
                WHERE identificacion_oferente = @identificacion_oferente
                ORDER BY id_experiencia DESC";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion_oferente", identificacionOferente);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ExperienciaLaboral
                            {
                                IdExperiencia = Convert.ToInt32(reader["id_experiencia"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                NombreEmpresa = reader["nombre_empresa"].ToString(),
                                PuestoDesempenado = reader["puesto_desempenado"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public ExperienciaLaboral ObtenerPorId(int idExperiencia)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    id_experiencia,
                    identificacion_oferente,
                    nombre_empresa,
                    puesto_desempenado,
                    fecha_inicio,
                    fecha_fin
                FROM experiencia_laboral
                WHERE id_experiencia = @id_experiencia";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_experiencia", idExperiencia);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ExperienciaLaboral
                            {
                                IdExperiencia = Convert.ToInt32(reader["id_experiencia"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                NombreEmpresa = reader["nombre_empresa"].ToString(),
                                PuestoDesempenado = reader["puesto_desempenado"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insertar(ExperienciaLaboral experiencia)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
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
                    @identificacion_oferente,
                    @nombre_empresa,
                    @puesto_desempenado,
                    @fecha_inicio,
                    @fecha_fin
                )";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion_oferente", experiencia.IdentificacionOferente);
                    cmd.Parameters.AddWithValue("@nombre_empresa", experiencia.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@puesto_desempenado", experiencia.PuestoDesempenado);
                    cmd.Parameters.AddWithValue("@fecha_inicio", experiencia.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", experiencia.FechaFin);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(ExperienciaLaboral experiencia)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE experiencia_laboral
                SET
                    identificacion_oferente = @identificacion_oferente,
                    nombre_empresa = @nombre_empresa,
                    puesto_desempenado = @puesto_desempenado,
                    fecha_inicio = @fecha_inicio,
                    fecha_fin = @fecha_fin
                WHERE id_experiencia = @id_experiencia";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_experiencia", experiencia.IdExperiencia);
                    cmd.Parameters.AddWithValue("@identificacion_oferente", experiencia.IdentificacionOferente);
                    cmd.Parameters.AddWithValue("@nombre_empresa", experiencia.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@puesto_desempenado", experiencia.PuestoDesempenado);
                    cmd.Parameters.AddWithValue("@fecha_inicio", experiencia.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", experiencia.FechaFin);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool TieneDatosRelacionados(int idExperiencia)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT COUNT(*)
                FROM experiencia_laboral_asignacion
                WHERE id_experiencia = @id_experiencia";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_experiencia", idExperiencia);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Eliminar(int idExperiencia)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                DELETE FROM experiencia_laboral
                WHERE id_experiencia = @id_experiencia";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_experiencia", idExperiencia);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}