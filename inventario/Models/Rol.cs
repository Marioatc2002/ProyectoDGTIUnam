using System.ComponentModel.DataAnnotations;

namespace inventario.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string NombreRol { get; set; }

        // --- Relaciones ---

        // Relación Uno a Muchos con Usuario (Un Rol pertenece a muchos Usuarios)
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}