using AdministraciondePersonal.Entities;
using Dapper;
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

                conn.Execute(sql, new
                {
                    usuario = usuario,
                    descripcion = descripcionAccion,
                    fecha = DateTime.Now
                });
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
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(filtroUsuario))
                {
                    whereClauses.Add("usuario LIKE @usuario");
                    parameters.Add("@usuario", $"%{filtroUsuario}%");
                }

                if (!string.IsNullOrEmpty(filtroDescripcion))
                {
                    whereClauses.Add("descripcion_accion LIKE @descripcion");
                    parameters.Add("@descripcion", $"%{filtroDescripcion}%");
                }

                string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

                string countSql = $"SELECT COUNT(*) FROM bitacora {whereSql}";
                totalCount = conn.ExecuteScalar<int>(countSql, parameters);

                string orderBy = ordenarPor switch
                {
                    "Usuario" => "usuario",
                    "FechaBitacora" => "fecha_bitacora",
                    _ => "fecha_bitacora"
                };

                string orderDirection = direccion.ToUpper() == "ASC" ? "ASC" : "DESC";

                int offset = (pageIndex - 1) * pageSize;

                string dataSql = $@"
                    SELECT 
                        id_bitacora AS IdBitacora, 
                        fecha_bitacora AS FechaBitacora, 
                        usuario AS Usuario, 
                        descripcion_accion AS DescripcionAccion
                    FROM bitacora {whereSql}
                    ORDER BY {orderBy} {orderDirection}
                    LIMIT @offset, @pageSize";

                parameters.Add("@offset", offset);
                parameters.Add("@pageSize", pageSize);

                items = conn.Query<Bitacora>(dataSql, parameters).ToList();
            }

            return (items, totalCount);
        }
        public List<string> ObtenerUsuariosUnicos()
        {
            using (IDbConnection conn = _dbFactory.GetConnection())
            {
                string sql = "SELECT DISTINCT usuario FROM bitacora ORDER BY usuario";
                return conn.Query<string>(sql).ToList();
            }
        }
    }
}