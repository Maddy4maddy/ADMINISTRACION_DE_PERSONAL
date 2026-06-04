using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class UsuarioRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public UsuarioRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Usuario ObtenerPorNombre(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT id_usuario, nombre_usuario, nombre_completo, contrasena, 
                                      intentos_fallidos, bloqueado, estado 
                               FROM usuarios 
                               WHERE nombre_usuario = @usuario";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                Contrasena = reader.GetString("contrasena"),
                                IntentosFallidos = reader.GetInt32("intentos_fallidos"),
                                Bloqueado = reader.GetBoolean("bloqueado"),
                                Estado = reader.GetString("estado")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public Usuario Login(string nombreUsuario, string contrasenaEncriptada)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT id_usuario, nombre_usuario, nombre_completo, contrasena, 
                                      intentos_fallidos, bloqueado, estado 
                               FROM usuarios 
                               WHERE nombre_usuario = @usuario 
                               AND contrasena = @contrasena 
                               AND bloqueado = FALSE";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasenaEncriptada);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                IntentosFallidos = reader.GetInt32("intentos_fallidos"),
                                Bloqueado = reader.GetBoolean("bloqueado"),
                                Estado = reader.GetString("estado")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void IncrementarIntentos(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"UPDATE usuarios 
                               SET intentos_fallidos = intentos_fallidos + 1 
                               WHERE nombre_usuario = @usuario";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Bloquear después de 3 intentos
                string sqlBloquear = @"UPDATE usuarios 
                                        SET bloqueado = TRUE, estado = 'bloqueado' 
                                        WHERE nombre_usuario = @usuario AND intentos_fallidos >= 3";

                using (var cmd = new MySqlCommand(sqlBloquear, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ResetearIntentos(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"UPDATE usuarios 
                               SET intentos_fallidos = 0, bloqueado = FALSE, estado = 'activo' 
                               WHERE nombre_usuario = @usuario";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}