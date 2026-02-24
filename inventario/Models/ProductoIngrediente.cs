namespace inventario.Models
{
    public class ProductoIngrediente
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public DateOnly FechaEntrada {get;set; }
        public DateOnly? FechaVencimiento {get;set; }

        public int Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal CantidadProducto { get; set; }
        public string UrlImagen { get; set; }
        public string Codigo { get; set; }

    }

}
