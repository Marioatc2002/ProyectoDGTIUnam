using System.ComponentModel.DataAnnotations;

namespace inventario.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TipoCategoria { get; set; }

        // --- Relaciones ---

        // Relación Uno a Muchos con Producto (Una Categoría tiene muchos Productos)
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}