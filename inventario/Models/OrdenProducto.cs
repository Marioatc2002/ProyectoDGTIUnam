using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventario.Models
{
    public class OrdenProducto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Cantidad { get; set; }

        // --- Relaciones (Llaves Foráneas de la relación N:N) ---

        // 1. Relación con Producto
        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        // 2. Relación con Orden
        [Required]
        public int OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public virtual Orden Orden { get; set; }
    }
}