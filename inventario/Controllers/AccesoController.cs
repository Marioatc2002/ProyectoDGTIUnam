using inventario.Data;
using inventario.Models;
using inventario.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace inventario.Controllers
{
    public class AccesoController : Controller
    {
        //private readonly AppDBContext _context;
        //public AccesoController(AppDBContext context)
        //{
        //    _context = context;
        //}

        //private void CargarListasUI(UsuarioVM? model = null)
        //{
          
        //    ViewBag.StatusList = new SelectList(new[] {
        //        new { Text = "Activo", Value = "A" },
        //        new { Text = "Inactivo", Value = "I" },
        //        new { Text = "Baja", Value = "B" }
        //    }, "Value", "Text", model?.Status.ToString());

        //}
        //[HttpGet]
        //public IActionResult Registrarse()
        //{
        //    CargarListasUI();

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        //{
        //    CargarListasUI();
        //    if (modelo.Pass != modelo.PassConfirm)
        //    { 
        //        ViewData["Error"] = "Las contraseñas no coinciden.";
        //        return View();
        //    }


        //    Usuario usuario = new Usuario()
        //    {
        //        Nombre = modelo.Nombre,
        //        ApellidoMaterno = modelo.ApellidoMaterno,
        //        ApellidoPaterno = modelo.ApellidoPaterno,
        //        Email = modelo.Email,
        //        Pass = modelo.Pass,
        //        Salt = modelo.Salt,
        //        Status = modelo.Status,
        //    =
        //        FechaAlta = DateTime.Now,
        //        FechaNacimiento = modelo.FechaNacimiento
        //    };
            
        //    await _context.Usuarios.AddAsync(usuario);
        //    await _context.SaveChangesAsync();

        //    if(usuario.Id != 0) return RedirectToAction("Login", "Acceso");

        //    return View();
        //}
    }
}
