using System;

namespace AdministraciondePersonal.Entities
{
    public class Entrevista
    {
        public int IdEntrevista { get; set; }
        public string IdentificacionOferente { get; set; }
        public string NombreOferente { get; set; }
        public int IdUsuarioEntrevistador { get; set; }
        public string NombreEntrevistador { get; set; }
        public DateTime FechaEntrevista { get; set; }
        public string Estado { get; set; }
    }
}