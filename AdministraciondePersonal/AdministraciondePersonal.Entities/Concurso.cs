using System;
using System.Collections.Generic;
using System.Text;

namespace AdministraciondePersonal.Entities
{
    public class Concurso
    {
        public int CodigoConcurso { get; set; }
        public string NombreConcurso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
    }
}
