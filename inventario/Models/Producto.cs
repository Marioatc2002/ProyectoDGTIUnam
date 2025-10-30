using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventario.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CodigoProducto { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // --- Relaciones ---

        // 1. Relación Muchos a Uno con Categoria (Un Producto tiene una Categoría)
        [Required]
        public int CategoriaId { get; set; } // Llave Foránea

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; } // Propiedad de Navegación

        // 2. Relación Muchos a Muchos con Orden (a través de OrdenProducto)
        public virtual ICollection<OrdenProducto> OrdenProductos { get; set; } = new List<OrdenProducto>();
    }
}