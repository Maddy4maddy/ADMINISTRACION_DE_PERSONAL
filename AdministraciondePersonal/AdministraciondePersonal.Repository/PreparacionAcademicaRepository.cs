using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
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
            List<PreparacionAcademica> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    pa.id_preparacion,
                    pa.identificacion_oferente,
                    pa.id_institucion,
                    ie.nombre_institucion,
                    pa.titulo_obtenido,
                    pa.fecha_inicio,
                    pa.fecha_fin
                FROM preparacion_academica pa
                INNER JOIN instituciones_educativas ie
                    ON pa.id_institucion = ie.id_institucion
                WHERE pa.identificacion_oferente = @identificacion_oferente
                ORDER BY pa.id_preparacion DESC";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion_oferente", identificacionOferente);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new PreparacionAcademica
                            {
                                IdPreparacion = Convert.ToInt32(reader["id_preparacion"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                IdInstitucion = Convert.ToInt32(reader["id_institucion"]),
                                NombreInstitucion = reader["nombre_institucion"].ToString(),
                                TituloObtenido = reader["titulo_obtenido"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public PreparacionAcademica ObtenerPorId(int idPreparacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    pa.id_preparacion,
                    pa.identificacion_oferente,
                    pa.id_institucion,
                    ie.nombre_institucion,
                    pa.titulo_obtenido,
                    pa.fecha_inicio,
                    pa.fecha_fin
                FROM preparacion_academica pa
                INNER JOIN instituciones_educativas ie
                    ON pa.id_institucion = ie.id_institucion
                WHERE pa.id_preparacion = @id_preparacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_preparacion", idPreparacion);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PreparacionAcademica
                            {
                                IdPreparacion = Convert.ToInt32(reader["id_preparacion"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                IdInstitucion = Convert.ToInt32(reader["id_institucion"]),
                                NombreInstitucion = reader["nombre_institucion"].ToString(),
                                TituloObtenido = reader["titulo_obtenido"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insertar(PreparacionAcademica preparacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
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
                    @identificacion_oferente,
                    @id_institucion,
                    @titulo_obtenido,
                    @fecha_inicio,
                    @fecha_fin
                )";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion_oferente", preparacion.IdentificacionOferente);
                    cmd.Parameters.AddWithValue("@id_institucion", preparacion.IdInstitucion);
                    cmd.Parameters.AddWithValue("@titulo_obtenido", preparacion.TituloObtenido);
                    cmd.Parameters.AddWithValue("@fecha_inicio", preparacion.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", preparacion.FechaFin);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(PreparacionAcademica preparacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE preparacion_academica
                SET
                    identificacion_oferente = @identificacion_oferente,
                    id_institucion = @id_institucion,
                    titulo_obtenido = @titulo_obtenido,
                    fecha_inicio = @fecha_inicio,
                    fecha_fin = @fecha_fin
                WHERE id_preparacion = @id_preparacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_preparacion", preparacion.IdPreparacion);
                    cmd.Parameters.AddWithValue("@identificacion_oferente", preparacion.IdentificacionOferente);
                    cmd.Parameters.AddWithValue("@id_institucion", preparacion.IdInstitucion);
                    cmd.Parameters.AddWithValue("@titulo_obtenido", preparacion.TituloObtenido);
                    cmd.Parameters.AddWithValue("@fecha_inicio", preparacion.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", preparacion.FechaFin);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool TieneDatosRelacionados(int idPreparacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT COUNT(*)
                FROM preparacion_academica_asignacion
                WHERE id_preparacion = @id_preparacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_preparacion", idPreparacion);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Eliminar(int idPreparacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                DELETE FROM preparacion_academica
                WHERE id_preparacion = @id_preparacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_preparacion", idPreparacion);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<InstitucionEducativa> ObtenerInstituciones()
        {
            List<InstitucionEducativa> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    id_institucion,
                    nombre_institucion
                FROM instituciones_educativas
                ORDER BY nombre_institucion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new InstitucionEducativa
                            {
                                IdInstitucion = Convert.ToInt32(reader["id_institucion"]),
                                NombreInstitucion = reader["nombre_institucion"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<Oferente> ObtenerOferentes()
        {
            List<Oferente> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    identificacion,
                    tipo_identificacion,
                    nombre_completo,
                    fecha_nacimiento,
                    correo,
                    telefono
                FROM oferentes
                ORDER BY nombre_completo";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Oferente
                            {
                                Identificacion = reader["identificacion"].ToString(),
                                TipoIdentificacion = reader["tipo_identificacion"].ToString(),
                                NombreCompleto = reader["nombre_completo"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                                Correo = reader["correo"].ToString(),
                                Telefono = reader["telefono"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}