namespace inventario.Models
{
    public class ProductoTienda
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int? Cantidad { get; set; }

        public DateOnly FechaCreacion { get; set; }
        public DateOnly FechaCaducidad{ get; set; }

        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

    }
}
