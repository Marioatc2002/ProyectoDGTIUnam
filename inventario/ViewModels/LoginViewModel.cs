using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace inventario.ViewModels
{
    public class LoginViewModel
    {
        [MaxLength(100)]
        [EmailAddress]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Ingresa un email válido.")]
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número o símbolo.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Pass { get; set; }
    }
}
