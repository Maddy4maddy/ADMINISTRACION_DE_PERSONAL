using System.ComponentModel.DataAnnotations;

namespace AdministraciondePersonal.Entities
{
    public class Compania
    {
        public int IdCompania { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        public string NombreCompania { get; set; } = string.Empty;
    }
}