using Microsoft.EntityFrameworkCore;
using inventario.Models;

namespace inventario.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> opciones) : base(opciones)
        {

        }

        // --- Registrar tus entidades (Modelos) como tablas ---

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenProducto> OrdenProductos { get; set; }
    }
}