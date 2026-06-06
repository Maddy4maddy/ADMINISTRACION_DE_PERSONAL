using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
using System.Data;

namespace AdministraciondePersonal.Repository
{
    public class InstitucionEducativaRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public InstitucionEducativaRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<InstitucionEducativa> ObtenerTodos()
        {
            List<InstitucionEducativa> lista = new();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    id_institucion,
                    nombre_institucion
                FROM instituciones_educativas
                ORDER BY id_institucion DESC";

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

        public InstitucionEducativa ObtenerPorId(int idInstitucion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT
                    id_institucion,
                    nombre_institucion
                FROM instituciones_educativas
                WHERE id_institucion = @id_institucion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_institucion", idInstitucion);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new InstitucionEducativa
                            {
                                IdInstitucion = Convert.ToInt32(reader["id_institucion"]),
                                NombreInstitucion = reader["nombre_institucion"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insertar(InstitucionEducativa institucion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                INSERT INTO instituciones_educativas
                (
                    nombre_institucion
                )
                VALUES
                (
                    @nombre_institucion
                )";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@nombre_institucion", institucion.NombreInstitucion);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(InstitucionEducativa institucion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                UPDATE instituciones_educativas
                SET nombre_institucion = @nombre_institucion
                WHERE id_institucion = @id_institucion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_institucion", institucion.IdInstitucion);
                    cmd.Parameters.AddWithValue("@nombre_institucion", institucion.NombreInstitucion);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool TieneDatosRelacionados(int idInstitucion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                SELECT COUNT(*)
                FROM preparacion_academica
                WHERE id_institucion = @id_institucion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_institucion", idInstitucion);

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public void Eliminar(int idInstitucion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"
                DELETE FROM instituciones_educativas
                WHERE id_institucion = @id_institucion";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@id_institucion", idInstitucion);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}