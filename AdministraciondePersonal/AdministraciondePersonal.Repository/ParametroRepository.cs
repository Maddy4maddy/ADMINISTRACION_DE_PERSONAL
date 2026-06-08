using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class ParametroRepository
    {
        private readonly string _connectionString;

        public ParametroRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Parametros> ObtenerParametros()
        {
            var lista = new List<Parametros>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM parametros ORDER BY codigo",
                conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Parametros
                {
                    IdParametro = reader.GetInt32("id_parametro"),
                    Codigo = reader.GetString("codigo"),
                    Valor = reader.GetString("valor")
                });
            }

            return lista;
        }

        public string ObtenerValor(string codigo)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT valor FROM parametros WHERE codigo = @codigo",
                conn);

            cmd.Parameters.AddWithValue("@codigo", codigo);

            var resultado = cmd.ExecuteScalar();

            return resultado?.ToString() ?? "";
        }

        public Parametros ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM parametros WHERE id_parametro = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Parametros
                {
                    IdParametro = reader.GetInt32("id_parametro"),
                    Codigo = reader.GetString("codigo"),
                    Valor = reader.GetString("valor")
                };
            }

            return null;
        }

        public void CrearParametro(string codigo, string valor)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO parametros
                (codigo, valor)
                VALUES
                (@codigo, @valor)", conn);

            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@valor", valor);

            cmd.ExecuteNonQuery();
        }

        public void EditarParametro(int id, string codigo, string valor)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE parametros
                SET codigo = @codigo,
                    valor = @valor
                WHERE id_parametro = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@valor", valor);

            cmd.ExecuteNonQuery();
        }

        public void EliminarParametro(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "DELETE FROM parametros WHERE id_parametro = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}