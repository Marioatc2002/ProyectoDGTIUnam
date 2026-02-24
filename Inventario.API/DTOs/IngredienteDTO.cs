namespace Inventario.API.DTOs
{
    public class IngredienteDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string UnidadMedida { get; set; }
        public DateOnly FechaEntrada { get; set; }
        public string UrlImagen { get; set; } // Nuevo campo requerido
    }
}