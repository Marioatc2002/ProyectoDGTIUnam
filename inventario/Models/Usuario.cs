using System.ComponentModel;
using Microsoft.EntityFrameworkCore; // Necesario para [Index]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [ForeignKey]

namespace inventario.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Pass), IsUnique = true)]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Nombre (s)")]
        public string Nombre { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [MaxLength(40)]
        [Required]
        [DisplayName("Apellido Materno")]
        public string ApellidoMaterno { get; set; }
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
       
        public string? Salt { get; set; }
        [Required]
        public char Status { get; set; } = 'A'; 

        [Required]
        [DisplayName("Alta")]
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Required]
        [DisplayName("Nacimiento")]
        public DateOnly FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números sin espacios ni guiones")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El teléfono debe tener exactamente 10 dígitos")]
        public string Telefono { get; set; }

        public string? FotoRuta { get; set; } 
                                              //--- Relaciones ---

        [Required]
        public int GeneroId { get; set; } // Llave Foránea
        [ForeignKey("GeneroId")]
        public virtual Genero? Genero{ get; set; } // Propiedad de Navegación


        // 1. Relación Muchos a Uno con Rol (Un usuario tiene un Rol)
        [Required]
        public int RolId { get; set; } // Llave Foránea

        [ForeignKey("RolId")]
        public virtual Rol? Rol { get; set; } // Propiedad de Navegación


    }
}