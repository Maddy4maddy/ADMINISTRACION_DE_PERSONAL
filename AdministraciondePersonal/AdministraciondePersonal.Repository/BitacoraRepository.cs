using AdministraciondePersonal.Entities;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace AdministraciondePersonal.Repository
{
    public class BitacoraRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public BitacoraRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public void Registrar(string usuario, string descripcionAccion)
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = @"INSERT INTO bitacora (usuario, descripcion_accion, fecha_bitacora) 
                               VALUES (@usuario, @descripcion, @fecha)";

                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@descripcion", descripcionAccion);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public (List<Bitacora> items, int totalCount) ObtenerBitacoras(
            int pageIndex = 1,
            int pageSize = 100,
            string ordenarPor = "FechaBitacora",
            string direccion = "DESC",
            string filtroUsuario = null,
            string filtroDescripcion = null)
        {
            var items = new List<Bitacora>();
            int totalCount = 0;

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                var whereClauses = new List<string>();
                var parameters = new List<MySqlParameter>();

                if (!string.IsNullOrEmpty(filtroUsuario))
                {
                    whereClauses.Add("usuario LIKE @usuario");
                    parameters.Add(new MySqlParameter("@usuario", $"%{filtroUsuario}%"));
                }

                if (!string.IsNullOrEmpty(filtroDescripcion))
                {
                    whereClauses.Add("descripcion_accion LIKE @descripcion");
                    parameters.Add(new MySqlParameter("@descripcion", $"%{filtroDescripcion}%"));
                }

                string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

                
                string countSql = $"SELECT COUNT(*) FROM bitacora {whereSql}";
                using (var cmd = new MySqlCommand(countSql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    conn.Open();
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                string orderBy = ordenarPor switch
                {
                    "Usuario" => "usuario",
                    "FechaBitacora" => "fecha_bitacora",
                    _ => "fecha_bitacora"
                };

                string orderDirection = direccion.ToUpper() == "ASC" ? "ASC" : "DESC";

                int offset = (pageIndex - 1) * pageSize;

                string dataSql = $@"
                    SELECT id_bitacora, fecha_bitacora, usuario, descripcion_accion
                    FROM bitacora {whereSql}
                    ORDER BY {orderBy} {orderDirection}
                    LIMIT @offset, @pageSize";

                using (var cmd = new MySqlCommand(dataSql, (MySqlConnection)conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Bitacora
                            {
                                IdBitacora = reader.GetInt32("id_bitacora"),
                                FechaBitacora = reader.GetDateTime("fecha_bitacora"),
                                Usuario = reader.GetString("usuario"),
                                DescripcionAccion = reader.GetString("descripcion_accion")
                            });
                        }
                    }
                }
            }

            return (items, totalCount);
        }
        public List<string> ObtenerUsuariosUnicos()
        {
            var usuarios = new List<string>();

            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT DISTINCT usuario FROM bitacora ORDER BY usuario";
                using (var cmd = new MySqlCommand(sql, (MySqlConnection)conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(reader.GetString("usuario"));
                        }
                    }
                }
            }

            return usuarios;
        }
    }
}