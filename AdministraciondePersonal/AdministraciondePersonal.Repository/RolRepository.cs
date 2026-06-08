using MySql.Data.MySqlClient;
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

        //Validación
        public bool RolTieneUsuarios(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM usuarios WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@id", idRol);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // Obtener Pantallas
        public List<Pantalla> ObtenerPantallas()
        {
            var lista = new List<Pantalla>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT id_pantalla, nombre_pantalla, ruta FROM Pantallas", conn);

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

        // Obtener Roles
        public List<rol> ObtenerRoles()
        {
            var lista = new List<rol>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT id_rol, nombre_rol FROM roles", conn);

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

        // 🔥 CREAR ROL
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

        // 🔥 EDITAR ROL
        public void EditarRol(int idRol, string nombreRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE roles
                SET nombre_rol = @nombre
                WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@nombre", nombreRol);
            cmd.Parameters.AddWithValue("@id", idRol);

            cmd.ExecuteNonQuery();
        }

        // 🔥 ELIMINAR PANTALLAS DEL ROL
        public void EliminarPantallasPorRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                DELETE FROM RolPantalla 
                WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@id", idRol);

            cmd.ExecuteNonQuery();
        }

        //Aasignar Pantalla a RoL
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

        // 🔥 Eliminar Rol
        public void EliminarRol(int idRol)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "DELETE FROM roles WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@id", idRol);

            cmd.ExecuteNonQuery();
        }

        public List<int> ObtenerPantallasPorRol(int idRol)
        {
            var lista = new List<int>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
        SELECT id_pantalla 
        FROM RolPantalla 
        WHERE id_rol = @id", conn);

            cmd.Parameters.AddWithValue("@id", idRol);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(reader.GetInt32("id_pantalla"));
            }

            return lista;
        }
    }
}