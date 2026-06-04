using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
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
            List<Oferente> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    o.identificacion,
                    o.tipo_identificacion,
                    o.nombre_completo,
                    o.fecha_nacimiento,
                    o.correo,
                    o.telefono,
                    c.codigo_concurso,
                    c.nombre_concurso
                FROM oferentes o
                LEFT JOIN oferente_concurso oc
                    ON o.identificacion = oc.identificacion_oferente
                LEFT JOIN concursos c
                    ON oc.codigo_concurso = c.codigo_concurso
                ORDER BY o.nombre_completo";

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
                                Telefono = reader["telefono"].ToString(),
                                CodigoConcurso = reader["codigo_concurso"] == DBNull.Value ? 0 : Convert.ToInt32(reader["codigo_concurso"]),
                                NombreConcurso = reader["nombre_concurso"] == DBNull.Value ? "" : reader["nombre_concurso"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public Oferente ObtenerPorIdentificacion(string identificacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    o.identificacion,
                    o.tipo_identificacion,
                    o.nombre_completo,
                    o.fecha_nacimiento,
                    o.correo,
                    o.telefono,
                    c.codigo_concurso,
                    c.nombre_concurso
                FROM oferentes o
                LEFT JOIN oferente_concurso oc
                    ON o.identificacion = oc.identificacion_oferente
                LEFT JOIN concursos c
                    ON oc.codigo_concurso = c.codigo_concurso
                WHERE o.identificacion = @identificacion
                LIMIT 1";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion", identificacion);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Oferente
                            {
                                Identificacion = reader["identificacion"].ToString(),
                                TipoIdentificacion = reader["tipo_identificacion"].ToString(),
                                NombreCompleto = reader["nombre_completo"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                                Correo = reader["correo"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                CodigoConcurso = reader["codigo_concurso"] == DBNull.Value ? 0 : Convert.ToInt32(reader["codigo_concurso"]),
                                NombreConcurso = reader["nombre_concurso"] == DBNull.Value ? "" : reader["nombre_concurso"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM oferentes WHERE identificacion = @identificacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion", identificacion);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Insertar(Oferente oferente)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                conn.Open();

                using (var transaction = ((MySqlConnection)conn).BeginTransaction())
                {
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
                            @identificacion,
                            @tipo_identificacion,
                            @nombre_completo,
                            @fecha_nacimiento,
                            @correo,
                            @telefono
                        )";

                        using (var cmd = new MySqlCommand(sqlOferente, (MySqlConnection)conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@identificacion", oferente.Identificacion);
                            cmd.Parameters.AddWithValue("@tipo_identificacion", oferente.TipoIdentificacion);
                            cmd.Parameters.AddWithValue("@nombre_completo", oferente.NombreCompleto);
                            cmd.Parameters.AddWithValue("@fecha_nacimiento", oferente.FechaNacimiento);
                            cmd.Parameters.AddWithValue("@correo", oferente.Correo);
                            cmd.Parameters.AddWithValue("@telefono", oferente.Telefono);

                            cmd.ExecuteNonQuery();
                        }

                        string sqlConcurso = @"
                        INSERT INTO oferente_concurso
                        (
                            identificacion_oferente,
                            codigo_concurso
                        )
                        VALUES
                        (
                            @identificacion,
                            @codigo_concurso
                        )";

                        using (var cmd = new MySqlCommand(sqlConcurso, (MySqlConnection)conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@identificacion", oferente.Identificacion);
                            cmd.Parameters.AddWithValue("@codigo_concurso", oferente.CodigoConcurso);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Actualizar(Oferente oferente)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                conn.Open();

                using (var transaction = ((MySqlConnection)conn).BeginTransaction())
                {
                    try
                    {
                        string sqlOferente = @"
                        UPDATE oferentes
                        SET
                            tipo_identificacion = @tipo_identificacion,
                            nombre_completo = @nombre_completo,
                            fecha_nacimiento = @fecha_nacimiento,
                            correo = @correo,
                            telefono = @telefono
                        WHERE identificacion = @identificacion";

                        using (var cmd = new MySqlCommand(sqlOferente, (MySqlConnection)conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@identificacion", oferente.Identificacion);
                            cmd.Parameters.AddWithValue("@tipo_identificacion", oferente.TipoIdentificacion);
                            cmd.Parameters.AddWithValue("@nombre_completo", oferente.NombreCompleto);
                            cmd.Parameters.AddWithValue("@fecha_nacimiento", oferente.FechaNacimiento);
                            cmd.Parameters.AddWithValue("@correo", oferente.Correo);
                            cmd.Parameters.AddWithValue("@telefono", oferente.Telefono);

                            cmd.ExecuteNonQuery();
                        }

                        string sqlEliminarConcurso = @"
                        DELETE FROM oferente_concurso
                        WHERE identificacion_oferente = @identificacion";

                        using (var cmd = new MySqlCommand(sqlEliminarConcurso, (MySqlConnection)conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@identificacion", oferente.Identificacion);
                            cmd.ExecuteNonQuery();
                        }

                        string sqlInsertarConcurso = @"
                        INSERT INTO oferente_concurso
                        (
                            identificacion_oferente,
                            codigo_concurso
                        )
                        VALUES
                        (
                            @identificacion,
                            @codigo_concurso
                        )";

                        using (var cmd = new MySqlCommand(sqlInsertarConcurso, (MySqlConnection)conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@identificacion", oferente.Identificacion);
                            cmd.Parameters.AddWithValue("@codigo_concurso", oferente.CodigoConcurso);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool TieneDatosRelacionados(string identificacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                (
                    SELECT COUNT(*)
                    FROM oferente_concurso
                    WHERE identificacion_oferente = @identificacion
                )
                +
                (
                    SELECT COUNT(*)
                    FROM preparacion_academica
                    WHERE identificacion_oferente = @identificacion
                )
                +
                (
                    SELECT COUNT(*)
                    FROM experiencia_laboral
                    WHERE identificacion_oferente = @identificacion
                ) AS total";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion", identificacion);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Eliminar(string identificacion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "DELETE FROM oferentes WHERE identificacion = @identificacion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion", identificacion);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Concurso> ObtenerConcursos()
        {
            List<Concurso> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    codigo_concurso,
                    nombre_concurso,
                    fecha_inicio,
                    fecha_fin,
                    estado
                FROM concursos
                ORDER BY nombre_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Concurso
                            {
                                CodigoConcurso = Convert.ToInt32(reader["codigo_concurso"]),
                                NombreConcurso = reader["nombre_concurso"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                Estado = reader["estado"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}