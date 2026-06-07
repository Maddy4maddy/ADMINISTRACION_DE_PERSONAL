using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministraciondePersonal.Entities
{
    public class Pantalla
    {
        public int IdPantalla { get; set; }
        public string NombrePantalla { get; set; }
        public string Ruta { get; set; }

        public bool Seleccionada { get; set; }
    }
}
