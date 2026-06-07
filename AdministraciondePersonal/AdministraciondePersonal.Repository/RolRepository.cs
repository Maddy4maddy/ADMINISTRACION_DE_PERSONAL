using MySql.Data.MySqlClient;
using AdministraciondePersonal.Repository;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class RolRepository
    {
        private readonly string _connectionString;

        public RolRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Pantalla> ObtenerPantallas()
        {
            var lista = new List<Pantalla>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("Select * FROM Pantallas", conn);
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
        public List<rol> ObtenerRoles()
        {
            var lista = new List<rol>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM roles", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new rol
                {
                    IdRol = reader.GetInt32("id_rol"),
                    NombreRol = reader.GetString("nombre_rol")
                });
            }
            return lista;
        }

        public int CrearRol(string nombreRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO roles (nombre_rol)
                VALUES (@nombre);
                SELECT LAST_INSERT_ID();", conn);

            cmd.Parameters.AddWithValue("@nombre", nombreRol);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void EliminarRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var check = new MySqlCommand(
                "SELECT COUNT(*) FROM usuarios WHERE id_rol = @id", conn);

            check.Parameters.AddWithValue("@id", idRol);

            int existe = Convert.ToInt32(check.ExecuteScalar());

            if (existe > 0)
                throw new Exception("No se puede eliminar un registro con datos relacionados.");

            var cmd1 = new MySqlCommand(
                "DELETE FROM RolPantalla WHERE id_rol = @id", conn);
            cmd1.Parameters.AddWithValue("@id", idRol);
            cmd1.ExecuteNonQuery();

            var cmd2 = new MySqlCommand(
                "DELETE FROM roles WHERE id_rol = @id", conn);
            cmd2.Parameters.AddWithValue("@id", idRol);
            cmd2.ExecuteNonQuery();
        }

        public void EditarRol(int idRol, string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "UPDATE roles SET nombre_rol = @nombre WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@id", idRol);

            cmd.ExecuteNonQuery();
        }

        public void AsignarPantalla(int idRol, int idPantalla)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO RolPantalla (id_rol, id_pantalla)
                VALUES (@rol, @pantalla)", conn);

            cmd.Parameters.AddWithValue("@rol", idRol);
            cmd.Parameters.AddWithValue("@pantalla", idPantalla);

            cmd.ExecuteNonQuery();
        }
    }
}
