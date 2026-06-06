using System;

namespace AdministraciondePersonal.Entities
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public DateTime FechaBitacora { get; set; }
        public string Usuario { get; set; }
        public string DescripcionAccion { get; set; }
    }
}