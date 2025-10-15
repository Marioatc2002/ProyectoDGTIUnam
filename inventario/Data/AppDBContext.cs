using Microsoft.EntityFrameworkCore;
using inventario.Models;

namespace inventario.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> opciones) : base(opciones)
        {

        }
        //Se defina las tablas con dbset para ser creadas en la base de datos
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region creacionTablaUsuario
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(tb =>
            {
                //se configura la primera columna de la base de datos  
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                //
                tb.Property(col => col.Nombre).HasMaxLength(40).IsRequired();
                tb.Property(col => col.ApellidoMaterno).HasMaxLength(40).IsRequired();
                tb.Property(col => col.ApellidoPaterno).HasMaxLength(40).IsRequired();
                tb.Property(col => col.Email).HasMaxLength(50).IsRequired();
                tb.Property(col => col.Salt).HasMaxLength(16).IsRequired();
                tb.Property(col => col.Status).IsRequired();
                tb.Property(col => col.Rol).HasConversion<int>().IsRequired();
                tb.Property(col => col.FechaNacimiento).IsRequired();


            });
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
        #endregion
        }


    }
    
}
