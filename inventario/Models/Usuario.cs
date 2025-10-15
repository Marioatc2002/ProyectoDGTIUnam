using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace inventario.Models
{
    
    public enum Rol
    {
        Usuario = 0,
        Administrador = 1 
    }
    public class Usuario
    {
       
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        public string Nombre { get; set; }
        [MaxLength(40)]
        [Required]
        public string ApellidoMaterno { get; set; }
        [MaxLength(40)]
        [Required]
        public string ApellidoPaterno { get; set; }
        [MaxLength(50)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número o símbolo.")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public char Status { get; set; } = 'A'; //Dropdownlist (A/I/B)
        [Required]
        public int Rol { get; set; }
        [Required]
        public DateTime FechaAlta { get; set; } = DateTime.Today;
        [DataType(DataType.DateTime)]     
        [Required]
        public DateTime FechaNacimiento { get; set; } 
    }
}
