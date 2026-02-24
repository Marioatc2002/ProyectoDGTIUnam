using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventario.Data; // Tu namespace del DbContext

namespace Inventario.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDBContext _context;

        public UsuariosController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Buscamos al usuario solo por correo
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Correo);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "El usuario no existe" });
            }

            // 2. Generamos el hash de la contraseña que recibimos del login
            // Usamos el Salt que ya tiene el usuario en la base de datos
            string hashCalculado = Seguridad.GenerarHash(request.Password, usuario.Salt);

            // 3. Comparamos los hashes (suponiendo que tu columna se llama 'pass')
            if (usuario.Pass == hashCalculado)
            {
                return Ok(new { mensaje = "Bienvenido", usuarioId = usuario.Id });
            }
            else
            {
                return Unauthorized(new { mensaje = "Contraseña incorrecta" });
            }
        }
    }

    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}