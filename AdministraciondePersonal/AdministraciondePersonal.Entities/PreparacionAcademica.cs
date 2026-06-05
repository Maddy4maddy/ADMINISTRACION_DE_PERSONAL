using System;

namespace AdministraciondePersonal.Entities
{
    public class PreparacionAcademica
    {
        public int IdPreparacion { get; set; }

        public string IdentificacionOferente { get; set; }

        public int IdInstitucion { get; set; }

        public string NombreInstitucion { get; set; }

        public string TituloObtenido { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}