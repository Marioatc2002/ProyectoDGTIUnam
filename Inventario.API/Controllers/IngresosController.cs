using Microsoft.AspNetCore.Mvc;
using inventario.Data;     // Namespace de tu AppDBContext
using inventario.Models;   // Namespace de tus modelos
using Inventario.API.DTOs;

namespace Inventario.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngresosController : ControllerBase
    {
        private readonly AppDBContext _context; // El nombre de tu clase

        public IngresosController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("registrar-lista")]
        public async Task<IActionResult> RegistrarIngreso([FromBody] List<IngredienteDTO> lista)
        {
            if (lista == null || !lista.Any()) return BadRequest("Lista vacía");
            var entidades = lista.Select(item => new ProductoIngrediente
            {
                Codigo = item.Codigo,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                CantidadProducto = item.Cantidad,
                PrecioUnidad = item.Precio,
                UnidadMedida = item.UnidadMedida,
                FechaEntrada = item.FechaEntrada,
                UrlImagen = item.UrlImagen // Asignación del campo faltante
            }).ToList();

            await _context.Ingredientes.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Guardado con éxito" });
        }
    }
}