using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventario.Models
{
    public class ingresoIngredientes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string MotivoEntrada { get; set; }
        [Required]     

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        // Relación con Usuario
        [ForeignKey("Usuario")]
        public int Id_Usuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        // Relación con el Producto/Ingrediente Perecedero
        [ForeignKey("ProductoIngrediente")]
        public int Id_ProductoPerecedero { get; set; }
        public virtual ProductoIngrediente ProductoIngrediente { get; set; }


    }
}
