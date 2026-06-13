using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace AdministraciondePersonal.Repository
{
    public class UsuarioRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public UsuarioRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private string EncriptarSHA2(string contrasena)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public List<Rol> ObtenerRolesPorUsuario(int idUsuario)
        {
            var roles = new List<Rol>();
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT r.id_rol, r.nombre_rol 
                               FROM roles r
                               INNER JOIN usuariorol ur ON r.id_rol = ur.id_rol
                               WHERE ur.id_usuario = @idUsuario
                               ORDER BY r.nombre_rol";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Rol
                            {
                                IdRol = reader.GetInt32("id_rol"),
                                NombreRol = reader.GetString("nombre_rol")
                            });
                        }
                    }
                }
            }
            return roles;
        }

        public Usuario ObtenerPorNombre(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT id_usuario, nombre_usuario, nombre_completo, correo, contrasena, 
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
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                Correo = reader.GetString("correo"),
                                Contrasena = reader.GetString("contrasena"),
                                IntentosFallidos = reader.GetInt32("intentos_fallidos"),
                                Bloqueado = reader.GetBoolean("bloqueado"),
                                Estado = reader.GetString("estado")
                            };

                            reader.Close();
                            usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);

                            return usuario;
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
                string sql = @"SELECT id_usuario, nombre_usuario, nombre_completo, correo, 
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
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                Correo = reader.GetString("correo"),
                                IntentosFallidos = reader.GetInt32("intentos_fallidos"),
                                Bloqueado = reader.GetBoolean("bloqueado"),
                                Estado = reader.GetString("estado")
                            };

                            reader.Close();
                            usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);

                            return usuario;
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
                string sql = "UPDATE usuarios SET intentos_fallidos = intentos_fallidos + 1 WHERE nombre_usuario = @usuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                string sqlBloquear = "UPDATE usuarios SET bloqueado = TRUE, estado = 'bloqueado' WHERE nombre_usuario = @usuario AND intentos_fallidos >= 3";
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
                string sql = "UPDATE usuarios SET intentos_fallidos = 0, bloqueado = FALSE, estado = 'activo' WHERE nombre_usuario = @usuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Rol> ObtenerTodosRoles()
        {
            var roles = new List<Rol>();
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT id_rol, nombre_rol FROM roles ORDER BY nombre_rol";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Rol
                            {
                                IdRol = reader.GetInt32("id_rol"),
                                NombreRol = reader.GetString("nombre_rol")
                            });
                        }
                    }
                }
            }
            return roles;
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                    SELECT u.id_usuario, u.nombre_usuario, u.nombre_completo, u.correo, u.estado
                    FROM usuarios u
                    ORDER BY u.id_usuario";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                Correo = reader.GetString("correo"),
                                Estado = reader.GetString("estado"),
                                Roles = new List<Rol>()
                            });
                        }
                    }
                }

                foreach (var usuario in usuarios)
                {
                    usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);
                }
            }
            return usuarios;
        }

        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                    SELECT u.id_usuario, u.nombre_usuario, u.nombre_completo, u.correo, u.estado
                    FROM usuarios u
                    WHERE u.id_usuario = @idUsuario";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                NombreCompleto = reader.GetString("nombre_completo"),
                                Correo = reader.GetString("correo"),
                                Estado = reader.GetString("estado")
                            };

                            reader.Close();
                            usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);

                            return usuario;
                        }
                    }
                }
            }
            return null;
        }

        public bool ExisteNombreUsuario(string nombreUsuario, int? idExcluir = null)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @nombreUsuario";
                if (idExcluir.HasValue) sql += " AND id_usuario != @idExcluir";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    if (idExcluir.HasValue) cmd.Parameters.AddWithValue("@idExcluir", idExcluir.Value);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool ExisteCorreo(string correo, int? idExcluir = null)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
                if (idExcluir.HasValue) sql += " AND id_usuario != @idExcluir";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);
                    if (idExcluir.HasValue) cmd.Parameters.AddWithValue("@idExcluir", idExcluir.Value);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public int CrearUsuario(Usuario usuario, string contrasena, List<int> rolesIds)
        {
            string contrasenaEncriptada = EncriptarSHA2(contrasena);
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                var mysqlConn = (MySqlConnection)conn;
                mysqlConn.Open();

                using (var transaction = mysqlConn.BeginTransaction())
                {
                    try
                    {
                        string sqlUsuario = @"INSERT INTO usuarios (nombre_usuario, nombre_completo, correo, contrasena, estado, intentos_fallidos, bloqueado) 
                                              VALUES (@nombreUsuario, @nombreCompleto, @correo, @contrasena, @estado, 0, FALSE);
                                              SELECT LAST_INSERT_ID();";

                        int idUsuario;
                        using (var cmd = new MySqlCommand(sqlUsuario, mysqlConn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                            cmd.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto ?? "");
                            cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                            cmd.Parameters.AddWithValue("@contrasena", contrasenaEncriptada);
                            cmd.Parameters.AddWithValue("@estado", usuario.Estado ?? "activo");
                            idUsuario = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (int idRol in rolesIds)
                        {
                            string sqlRol = "INSERT INTO usuariorol (id_usuario, id_rol) VALUES (@idUsuario, @idRol)";
                            using (var cmd = new MySqlCommand(sqlRol, mysqlConn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                                cmd.Parameters.AddWithValue("@idRol", idRol);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return idUsuario;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"UPDATE usuarios 
                               SET nombre_usuario = @nombreUsuario, nombre_completo = @nombreCompleto, 
                                   correo = @correo, estado = @estado
                               WHERE id_usuario = @idUsuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarUsuarioConContrasena(Usuario usuario, string nuevaContrasena)
        {
            string contrasenaEncriptada = EncriptarSHA2(nuevaContrasena);
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"UPDATE usuarios 
                               SET nombre_usuario = @nombreUsuario, nombre_completo = @nombreCompleto, 
                                   correo = @correo, contrasena = @contrasena, estado = @estado
                               WHERE id_usuario = @idUsuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@contrasena", contrasenaEncriptada);
                    cmd.Parameters.AddWithValue("@estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarRolesUsuario(int idUsuario, List<int> nuevosRolesIds)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                var mysqlConn = (MySqlConnection)conn;
                mysqlConn.Open();

                using (var transaction = mysqlConn.BeginTransaction())
                {
                    try
                    {
                        string sqlDelete = "DELETE FROM usuariorol WHERE id_usuario = @idUsuario";
                        using (var cmd = new MySqlCommand(sqlDelete, mysqlConn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (int idRol in nuevosRolesIds)
                        {
                            string sqlInsert = "INSERT INTO usuariorol (id_usuario, id_rol) VALUES (@idUsuario, @idRol)";
                            using (var cmd = new MySqlCommand(sqlInsert, mysqlConn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                                cmd.Parameters.AddWithValue("@idRol", idRol);
                                cmd.ExecuteNonQuery();
                            }
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

        public void EliminarUsuario(int idUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "DELETE FROM usuarios WHERE id_usuario = @idUsuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool TieneRegistrosRelacionados(int idUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM oferentes WHERE correo = (SELECT correo FROM usuarios WHERE id_usuario = @idUsuario)";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void CambiarEstadoUsuario(int idUsuario, string nuevoEstado)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "UPDATE usuarios SET estado = @estado WHERE id_usuario = @idUsuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}