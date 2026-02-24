using inventario.Models;
using System.Data;

namespace inventario.Service
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string contrasena)
        {
            throw new NotImplementedException();
        }


        Task<Usuario> SaveUsuario(Usuario usuario)
        {
            throw new NotImplementedException();

        }

    }

}
