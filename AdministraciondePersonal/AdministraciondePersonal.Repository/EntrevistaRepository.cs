using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
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
            List<Entrevista> lista = new();
            int offset = (pagina - 1) * tamanioPagina;

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT 
                    e.id_entrevista,
                    e.identificacion_oferente,
                    o.nombre_completo AS nombre_oferente,
                    e.id_usuario_entrevistador,
                    u.nombre_completo AS nombre_entrevistador,
                    e.fecha_entrevista,
                    e.estado
                FROM entrevistas e
                INNER JOIN oferentes o 
                    ON e.identificacion_oferente = o.identificacion
                INNER JOIN usuarios u 
                    ON e.id_usuario_entrevistador = u.id_usuario
                ORDER BY e.fecha_entrevista ASC
                LIMIT @tamanioPagina OFFSET @offset";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@tamanioPagina", tamanioPagina);
                    cmd.Parameters.AddWithValue("@offset", offset);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Entrevista
                            {
                                IdEntrevista = Convert.ToInt32(reader["id_entrevista"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                NombreOferente = reader["nombre_oferente"].ToString(),
                                IdUsuarioEntrevistador = Convert.ToInt32(reader["id_usuario_entrevistador"]),
                                NombreEntrevistador = reader["nombre_entrevistador"].ToString(),
                                FechaEntrevista = Convert.ToDateTime(reader["fecha_entrevista"]),
                                Estado = reader["estado"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public int ContarEntrevistas()
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM entrevistas";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public Entrevista ObtenerPorId(int idEntrevista)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT 
                    e.id_entrevista,
                    e.identificacion_oferente,
                    o.nombre_completo AS nombre_oferente,
                    e.id_usuario_entrevistador,
                    u.nombre_completo AS nombre_entrevistador,
                    e.fecha_entrevista,
                    e.estado
                FROM entrevistas e
                INNER JOIN oferentes o 
                    ON e.identificacion_oferente = o.identificacion
                INNER JOIN usuarios u 
                    ON e.id_usuario_entrevistador = u.id_usuario
                WHERE e.id_entrevista = @id_entrevista";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_entrevista", idEntrevista);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Entrevista
                            {
                                IdEntrevista = Convert.ToInt32(reader["id_entrevista"]),
                                IdentificacionOferente = reader["identificacion_oferente"].ToString(),
                                NombreOferente = reader["nombre_oferente"].ToString(),
                                IdUsuarioEntrevistador = Convert.ToInt32(reader["id_usuario_entrevistador"]),
                                NombreEntrevistador = reader["nombre_entrevistador"].ToString(),
                                FechaEntrevista = Convert.ToDateTime(reader["fecha_entrevista"]),
                                Estado = reader["estado"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insertar(Entrevista entrevista)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
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
                    @identificacion_oferente,
                    @id_usuario_entrevistador,
                    @fecha_entrevista,
                    'Pendiente'
                )";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@identificacion_oferente", entrevista.IdentificacionOferente);
                    cmd.Parameters.AddWithValue("@id_usuario_entrevistador", entrevista.IdUsuarioEntrevistador);
                    cmd.Parameters.AddWithValue("@fecha_entrevista", entrevista.FechaEntrevista);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Entrevista entrevista)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE entrevistas
                SET
                    id_usuario_entrevistador = @id_usuario_entrevistador,
                    fecha_entrevista = @fecha_entrevista
                WHERE id_entrevista = @id_entrevista";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_entrevista", entrevista.IdEntrevista);
                    cmd.Parameters.AddWithValue("@id_usuario_entrevistador", entrevista.IdUsuarioEntrevistador);
                    cmd.Parameters.AddWithValue("@fecha_entrevista", entrevista.FechaEntrevista);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idEntrevista)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "DELETE FROM entrevistas WHERE id_entrevista = @id_entrevista";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_entrevista", idEntrevista);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MarcarComoRealizada(int idEntrevista)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE entrevistas
                SET estado = 'Realizada'
                WHERE id_entrevista = @id_entrevista";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_entrevista", idEntrevista);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Oferente> ObtenerOferentes()
        {
            List<Oferente> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT identificacion, nombre_completo
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
                                NombreCompleto = reader["nombre_completo"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<Usuario> ObtenerEntrevistadores()
        {
            List<Usuario> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT id_usuario, nombre_usuario, nombre_completo
                FROM usuarios
                WHERE estado = 'activo'
                ORDER BY nombre_completo";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                NombreUsuario = reader["nombre_usuario"].ToString(),
                                NombreCompleto = reader["nombre_completo"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}