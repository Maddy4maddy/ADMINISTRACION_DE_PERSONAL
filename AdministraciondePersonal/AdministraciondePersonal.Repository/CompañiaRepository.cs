using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using AdministraciondePersonal.Entities;

namespace AdministraciondePersonal.Repository
{
    public class CompaniaRepository
    {
        private readonly string _connectionString;

        public CompaniaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Compania> ObtenerCompanias()
        {
            var lista = new List<Compania>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT id_compania, nombre_compania FROM companias",
                conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Compania
                {
                    IdCompania = reader.GetInt32("id_compania"),
                    NombreCompania = reader.GetString("nombre_compania")
                });
            }

            return lista;
        }

        public void CrearCompania(string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO companias(nombre_compania)
                VALUES(@nombre)", conn);

            cmd.Parameters.AddWithValue("@nombre", nombre);

            cmd.ExecuteNonQuery();
        }

        public void EditarCompania(int id, string nombre)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE companias
                SET nombre_compania = @nombre
                WHERE id_compania = @id", conn);

            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public void EliminarCompania(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "DELETE FROM companias WHERE id_compania = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public bool CompaniaTieneDatosRelacionados(int id)
        {
            return false;
        }
    }
}