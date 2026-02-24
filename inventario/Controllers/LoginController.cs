using inventario.Data;
using inventario.Herramientas;
using inventario.Models;
using inventario.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace inventario.Controllers
{    

    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly AppDBContext _context;
        private readonly ISmsService _smsService; 
        private readonly IDistributedCache _cache; 
        private readonly IEmailService _mail; 

        public LoginController(IUsuarioService usuarioService, AppDBContext context, ISmsService smsService, IDistributedCache cache, IEmailService mail)
        {
            _usuarioService = usuarioService;
            _context = context;
            _smsService = smsService;
            _cache = cache;
            _mail = mail;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult IniciarSesion()
        { 
        
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave) 
        {

            var usuarioSalt = await _context.Usuarios
                .Where(u => u.Email == correo)
                .Select(u => u.Salt).FirstOrDefaultAsync();
            if (usuarioSalt == null)
            {
                ViewData["Mensaje"] = "El correo no está registrado";
                return View(); // Esto simplemente vuelve a cargar la página de Login actual
            }

            Usuario usuarioEncontrado = await _usuarioService.GetUsuario(correo, Seguridad.GenerarHash(clave, usuarioSalt));

            if (usuarioEncontrado != null)
            {
                // Lógica para iniciar sesión, como establecer cookies o sesiones
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
                    new Claim("Foto perfil", usuarioEncontrado.FotoRuta),
                    new Claim("RolId", usuarioEncontrado.RolId.ToString()),
                    new Claim("UsuarioId", usuarioEncontrado.Id.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                    );

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewData["Mensaje"] = "Usuario Encontrado";
                return View();
            }





        }


        [HttpGet]
        public IActionResult Recuperacion()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Recuperacion(string correo)
        {

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == correo);

            if (usuario == null || string.IsNullOrEmpty(usuario.Telefono))
            {
                ViewData["Mensaje"] = "Usuario no encontrado o no tiene teléfono registrado.";
                return View();
            }

      
            var code = new Random().Next(100000, 999999).ToString();

    
            await _cache.SetStringAsync($"OTP_{correo}", code, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

         
            await _mail.SendOtpEmailAsync(usuario.Email, code);


            return RedirectToAction("VerificarCodigo", new { correo = correo });
        }


        [HttpGet]
        public IActionResult VerificarCodigo(string correo)
        {
            ViewBag.Correo = correo;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerificarCodigo(string correo, string codigoIngresado)
        {

            var codigoGuardado = await _cache.GetStringAsync($"OTP_{correo}");

            if (codigoGuardado != null && codigoGuardado == codigoIngresado)
            {
         
                await _cache.RemoveAsync($"OTP_{correo}");

             
                return RedirectToAction("CambiarClave", new { correo = correo });
            }

         
            ViewBag.Correo = correo;
            ViewData["Mensaje"] = "El código es incorrecto o ha expirado.";
            return View();
        }

        [HttpGet]
        public IActionResult CambiarClave(string correo)
        {
            ViewBag.Correo = correo;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarClave(string correo, string nuevaClave, string confirmarClave)
        {
            if (nuevaClave != confirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                ViewBag.Correo = correo;
                return View();
            }


            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == correo);
            if (usuario != null)
            {
   
                usuario.Pass = Seguridad.GenerarHash(nuevaClave, usuario.Salt);

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "Contraseña actualizada correctamente. Ya puedes iniciar sesión.";
                return RedirectToAction("IniciarSesion");
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
