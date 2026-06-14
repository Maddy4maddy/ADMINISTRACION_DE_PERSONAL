using AdministraciondePersonal.Entities;
using Dapper;
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
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT r.id_rol AS IdRol, r.nombre_rol AS NombreRol 
                               FROM roles r
                               INNER JOIN usuariorol ur ON r.id_rol = ur.id_rol
                               WHERE ur.id_usuario = @idUsuario
                               ORDER BY r.nombre_rol";

                return conn.Query<Rol>(sql, new { idUsuario = idUsuario }).ToList();
            }
        }

        public Usuario ObtenerPorNombre(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT 
                                    id_usuario AS IdUsuario, 
                                    nombre_usuario AS NombreUsuario, 
                                    nombre_completo AS NombreCompleto, 
                                    correo AS Correo, 
                                    contrasena AS Contrasena, 
                                    intentos_fallidos AS IntentosFallidos, 
                                    bloqueado AS Bloqueado, 
                                    estado AS Estado
                               FROM usuarios 
                               WHERE nombre_usuario = @usuario";

                var usuario = conn.QueryFirstOrDefault<Usuario>(sql, new { usuario = nombreUsuario });

                if (usuario != null)
                {
                    usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);
                }

                return usuario;
            }
        }

        public Usuario Login(string nombreUsuario, string contrasenaEncriptada)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"SELECT 
                                    id_usuario AS IdUsuario, 
                                    nombre_usuario AS NombreUsuario, 
                                    nombre_completo AS NombreCompleto, 
                                    correo AS Correo, 
                                    intentos_fallidos AS IntentosFallidos, 
                                    bloqueado AS Bloqueado, 
                                    estado AS Estado
                               FROM usuarios 
                               WHERE nombre_usuario = @usuario 
                               AND contrasena = @contrasena 
                               AND bloqueado = FALSE";

                var usuario = conn.QueryFirstOrDefault<Usuario>(sql, new { usuario = nombreUsuario, contrasena = contrasenaEncriptada });

                if (usuario != null)
                {
                    usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);
                }

                return usuario;
            }
        }

        public void IncrementarIntentos(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "UPDATE usuarios SET intentos_fallidos = intentos_fallidos + 1 WHERE nombre_usuario = @usuario";
                conn.Execute(sql, new { usuario = nombreUsuario });

                string sqlBloquear = "UPDATE usuarios SET bloqueado = TRUE, estado = 'bloqueado' WHERE nombre_usuario = @usuario AND intentos_fallidos >= 3";
                conn.Execute(sqlBloquear, new { usuario = nombreUsuario });
            }
        }

        public void ResetearIntentos(string nombreUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "UPDATE usuarios SET intentos_fallidos = 0, bloqueado = FALSE, estado = 'activo' WHERE nombre_usuario = @usuario";
                conn.Execute(sql, new { usuario = nombreUsuario });
            }
        }

        public List<Rol> ObtenerTodosRoles()
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT id_rol AS IdRol, nombre_rol AS NombreRol FROM roles ORDER BY nombre_rol";
                return conn.Query<Rol>(sql).ToList();
            }
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                    SELECT 
                        u.id_usuario AS IdUsuario, 
                        u.nombre_usuario AS NombreUsuario, 
                        u.nombre_completo AS NombreCompleto, 
                        u.correo AS Correo, 
                        u.estado AS Estado
                    FROM usuarios u
                    ORDER BY u.id_usuario";

                var usuarios = conn.Query<Usuario>(sql).ToList();

                foreach (var usuario in usuarios)
                {
                    usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);
                }

                return usuarios;
            }
        }

        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                    SELECT 
                        u.id_usuario AS IdUsuario, 
                        u.nombre_usuario AS NombreUsuario, 
                        u.nombre_completo AS NombreCompleto, 
                        u.correo AS Correo, 
                        u.estado AS Estado
                    FROM usuarios u
                    WHERE u.id_usuario = @idUsuario";

                var usuario = conn.QueryFirstOrDefault<Usuario>(sql, new { idUsuario = idUsuario });

                if (usuario != null)
                {
                    usuario.Roles = ObtenerRolesPorUsuario(usuario.IdUsuario);
                }

                return usuario;
            }
        }

        public bool ExisteNombreUsuario(string nombreUsuario, int? idExcluir = null)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @nombreUsuario";
                if (idExcluir.HasValue) sql += " AND id_usuario != @idExcluir";

                var parameters = new { nombreUsuario = nombreUsuario, idExcluir = idExcluir };
                return conn.ExecuteScalar<int>(sql, parameters) > 0;
            }
        }

        public bool ExisteCorreo(string correo, int? idExcluir = null)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
                if (idExcluir.HasValue) sql += " AND id_usuario != @idExcluir";

                var parameters = new { correo = correo, idExcluir = idExcluir };
                return conn.ExecuteScalar<int>(sql, parameters) > 0;
            }
        }

        public int CrearUsuario(Usuario usuario, string contrasena, List<int> rolesIds)
        {
            string contrasenaEncriptada = EncriptarSHA2(contrasena);
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlUsuario = @"INSERT INTO usuarios (nombre_usuario, nombre_completo, correo, contrasena, estado, intentos_fallidos, bloqueado) 
                                              VALUES (@nombreUsuario, @nombreCompleto, @correo, @contrasena, @estado, 0, FALSE);
                                              SELECT LAST_INSERT_ID();";

                        int idUsuario = conn.ExecuteScalar<int>(sqlUsuario, new
                        {
                            nombreUsuario = usuario.NombreUsuario,
                            nombreCompleto = usuario.NombreCompleto ?? "",
                            correo = usuario.Correo,
                            contrasena = contrasenaEncriptada,
                            estado = usuario.Estado ?? "activo"
                        }, transaction);

                        foreach (int idRol in rolesIds)
                        {
                            string sqlRol = "INSERT INTO usuariorol (id_usuario, id_rol) VALUES (@idUsuario, @idRol)";
                            conn.Execute(sqlRol, new { idUsuario = idUsuario, idRol = idRol }, transaction);
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

                conn.Execute(sql, new
                {
                    nombreUsuario = usuario.NombreUsuario,
                    nombreCompleto = usuario.NombreCompleto,
                    correo = usuario.Correo,
                    estado = usuario.Estado,
                    idUsuario = usuario.IdUsuario
                });
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

                conn.Execute(sql, new
                {
                    nombreUsuario = usuario.NombreUsuario,
                    nombreCompleto = usuario.NombreCompleto,
                    correo = usuario.Correo,
                    contrasena = contrasenaEncriptada,
                    estado = usuario.Estado,
                    idUsuario = usuario.IdUsuario
                });
            }
        }

        public void ActualizarRolesUsuario(int idUsuario, List<int> nuevosRolesIds)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlDelete = "DELETE FROM usuariorol WHERE id_usuario = @idUsuario";
                        conn.Execute(sqlDelete, new { idUsuario = idUsuario }, transaction);

                        foreach (int idRol in nuevosRolesIds)
                        {
                            string sqlInsert = "INSERT INTO usuariorol (id_usuario, id_rol) VALUES (@idUsuario, @idRol)";
                            conn.Execute(sqlInsert, new { idUsuario = idUsuario, idRol = idRol }, transaction);
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
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                       
                        string sqlEliminarRoles = "DELETE FROM usuariorol WHERE id_usuario = @idUsuario";
                        conn.Execute(sqlEliminarRoles, new { idUsuario = idUsuario }, transaction);

                        
                        string sqlEliminarUsuario = "DELETE FROM usuarios WHERE id_usuario = @idUsuario";
                        int filasAfectadas = conn.Execute(sqlEliminarUsuario, new { idUsuario = idUsuario }, transaction);

                        if (filasAfectadas == 0)
                            throw new Exception("No se encontró el usuario a eliminar");

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

        public bool TieneRegistrosRelacionados(int idUsuario)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                    SELECT 
                        (SELECT COUNT(*) FROM usuariorol WHERE id_usuario = @idUsuario) +
                        (SELECT COUNT(*) FROM bitacora WHERE usuario = (SELECT nombre_usuario FROM usuarios WHERE id_usuario = @idUsuario)) +
                        (SELECT COUNT(*) FROM oferentes WHERE correo = (SELECT correo FROM usuarios WHERE id_usuario = @idUsuario))
                    AS TotalRegistros";

                return conn.ExecuteScalar<int>(sql, new { idUsuario = idUsuario }) > 0;
            }
        }

        public void CambiarEstadoUsuario(int idUsuario, string nuevoEstado)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "UPDATE usuarios SET estado = @estado WHERE id_usuario = @idUsuario";
                conn.Execute(sql, new { estado = nuevoEstado, idUsuario = idUsuario });
            }
        }
    }
}