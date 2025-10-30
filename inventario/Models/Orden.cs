using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventario.Models
{
    public class Orden
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string CodigoOrden { get; set; }
        [Required]
        [StringLength(300)]
        public string DireccionEntrega { get; set; }
        [Required]
        public char Status { get; set; }

        public DateTime FechaSolicitada { get; set; } = DateTime.Now;
        public DateTime FechaEntrega { get; set; }

        // --- Relaciones ---

        // 1. Relación Muchos a Uno con Usuario (Una Orden pertenece a un Usuario)
        [Required]
        public int UsuarioId { get; set; } // Llave Foránea

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } // Propiedad de Navegación

        // 2. Relación Muchos a Muchos con Producto (a través de OrdenProducto)
        public virtual ICollection<OrdenProducto> OrdenProductos { get; set; } = new List<OrdenProducto>();
    }
}