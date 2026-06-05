using System;

namespace AdministraciondePersonal.Entities
{
    public class ExperienciaLaboral
    {
        public int IdExperiencia { get; set; }

        public string IdentificacionOferente { get; set; }

        public string NombreEmpresa { get; set; }

        public string PuestoDesempenado { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}