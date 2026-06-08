using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class PantallaRepository
    {
        private readonly string _connectionString;

        public PantallaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Pantalla> ObtenerPantallas()
        {
            var lista = new List<Pantalla>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM pantallas ORDER BY nombre_pantalla",
                conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Pantalla
                {
                    IdPantalla = reader.GetInt32("id_pantalla"),
                    NombrePantalla = reader.GetString("nombre_pantalla"),
                    Ruta = reader.GetString("ruta")
                });
            }

            return lista;
        }

        public Pantalla ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM pantallas WHERE id_pantalla = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Pantalla
                {
                    IdPantalla = reader.GetInt32("id_pantalla"),
                    NombrePantalla = reader.GetString("nombre_pantalla"),
                    Ruta = reader.GetString("ruta")
                };
            }

            return null;
        }

        public Pantalla ObtenerPorRuta(string ruta)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM pantallas WHERE ruta = @ruta",
                conn);

            cmd.Parameters.AddWithValue("@ruta", ruta);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Pantalla
                {
                    IdPantalla = reader.GetInt32("id_pantalla"),
                    NombrePantalla = reader.GetString("nombre_pantalla"),
                    Ruta = reader.GetString("ruta")
                };
            }

            return null;
        }

        public void CrearPantalla(string nombrePantalla, string ruta)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO pantallas
                (nombre_pantalla, ruta)
                VALUES
                (@nombre, @ruta)", conn);

            cmd.Parameters.AddWithValue("@nombre", nombrePantalla);
            cmd.Parameters.AddWithValue("@ruta", ruta);

            cmd.ExecuteNonQuery();
        }

        public void EditarPantalla(int id, string nombrePantalla, string ruta)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE pantallas
                SET nombre_pantalla = @nombre,
                    ruta = @ruta
                WHERE id_pantalla = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nombre", nombrePantalla);
            cmd.Parameters.AddWithValue("@ruta", ruta);

            cmd.ExecuteNonQuery();
        }

        public bool TieneRolesAsignados(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT COUNT(*)
                FROM rolpantalla
                WHERE id_pantalla = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

            return cantidad > 0;
        }

        public void EliminarPantalla(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "DELETE FROM pantallas WHERE id_pantalla = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
