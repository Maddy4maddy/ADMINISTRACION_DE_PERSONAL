using System;
using System.Collections.Generic;
using System.Linq;

namespace AdministraciondePersonal.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
        public string Estado { get; set; }

        // Múltiples roles
        public List<Rol> Roles { get; set; } = new List<Rol>();

        //mostrar roles como string
        public string RolesTexto => string.Join(", ", Roles.Select(r => r.NombreRol));
    }

    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
    }
}