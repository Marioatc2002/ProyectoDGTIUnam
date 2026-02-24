using Microsoft.EntityFrameworkCore;
using inventario.Models;

namespace inventario.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        // --- Registrar tus entidades (Modelos) como tablas ---
        public DbSet<ingresoIngredientes> IngresoIngredientes { get; set; }
        public DbSet<ProductoIngrediente> Ingredientes { get; set; }
        public DbSet<ProductoTienda> ProductosTienda { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Genero> Genero { get; set; }
    }
}