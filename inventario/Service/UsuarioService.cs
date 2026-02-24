using inventario.Data;
using inventario.Models;
using Microsoft.EntityFrameworkCore;

namespace inventario.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDBContext _context;

        public UsuarioService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuario(string correo, string contrasena)
        {
            Usuario? usuario = await _context.Usuarios.Where(u => u.Email == correo && u.Pass == contrasena).FirstOrDefaultAsync();
            return usuario;
        }


        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;

        }

    }

}
