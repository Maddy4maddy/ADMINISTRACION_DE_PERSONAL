
using System;
using System.Collections.Generic;
using System.Text;

namespace AdministraciondePersonal.Entities
{
    public class Oferente
    {
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public int CodigoConcurso { get; set; }
        public string NombreConcurso { get; set; }
    }
}
