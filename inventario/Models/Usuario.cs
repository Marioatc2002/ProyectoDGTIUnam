using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [ForeignKey]

namespace inventario.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Nombres")]
        public string Nombre { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Apellido Materno")]
        public string ApellidoMaterno { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número o símbolo.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Pass { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public char Status { get; set; } = 'A'; //Dropdownlist (A/I/B)

        [Required]
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        [DataType(DataType.DateTime)]
        [Required]
        [DisplayName("Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        // --- Relaciones ---

        // 1. Relación Muchos a Uno con Rol (Un usuario tiene un Rol)
        [Required]
        public int RolId { get; set; } // Llave Foránea

        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; } // Propiedad de Navegación

        // 2. Relación Uno a Muchos con Orden (Un usuario tiene muchas Ordenes)
        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}