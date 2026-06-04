using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
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
            List<Concurso> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT codigo_concurso, nombre_concurso, fecha_inicio, fecha_fin, estado
                FROM concursos
                ORDER BY codigo_concurso DESC";

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

        public Concurso ObtenerPorCodigo(int codigoConcurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT codigo_concurso, nombre_concurso, fecha_inicio, fecha_fin, estado
                FROM concursos
                WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", codigoConcurso);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Concurso
                            {
                                CodigoConcurso = Convert.ToInt32(reader["codigo_concurso"]),
                                NombreConcurso = reader["nombre_concurso"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                Estado = reader["estado"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteCodigo(int codigoConcurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM concursos WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", codigoConcurso);
                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Insertar(Concurso concurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
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
                    @codigo_concurso,
                    @nombre_concurso,
                    @fecha_inicio,
                    @fecha_fin,
                    @estado
                )";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", concurso.CodigoConcurso);
                    cmd.Parameters.AddWithValue("@nombre_concurso", concurso.NombreConcurso);
                    cmd.Parameters.AddWithValue("@fecha_inicio", concurso.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", concurso.FechaFin);
                    cmd.Parameters.AddWithValue("@estado", concurso.Estado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Concurso concurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE concursos
                SET
                    nombre_concurso = @nombre_concurso,
                    fecha_inicio = @fecha_inicio,
                    fecha_fin = @fecha_fin,
                    estado = @estado
                WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", concurso.CodigoConcurso);
                    cmd.Parameters.AddWithValue("@nombre_concurso", concurso.NombreConcurso);
                    cmd.Parameters.AddWithValue("@fecha_inicio", concurso.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", concurso.FechaFin);
                    cmd.Parameters.AddWithValue("@estado", concurso.Estado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool TieneDatosRelacionados(int codigoConcurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT COUNT(*)
                FROM oferente_concurso
                WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", codigoConcurso);
                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Eliminar(int codigoConcurso)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "DELETE FROM concursos WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", codigoConcurso);
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarEstado(int codigoConcurso, string estado)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE concursos
                SET estado = @estado
                WHERE codigo_concurso = @codigo_concurso";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@codigo_concurso", codigoConcurso);
                    cmd.Parameters.AddWithValue("@estado", estado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}