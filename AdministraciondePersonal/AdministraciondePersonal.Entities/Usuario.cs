using System;

namespace AdministraciondePersonal.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Contrasena { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
        public string Estado { get; set; }
    }
}