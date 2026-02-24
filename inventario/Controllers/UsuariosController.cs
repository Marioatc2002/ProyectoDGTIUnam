using inventario.Data;
using inventario.Herramientas; // Asegúrate que aquí esté tu clase de Seguridad
using inventario.Models;
using inventario.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventario.Infrastructure;

namespace inventario.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsuariosController(AppDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Método privado para centralizar la carga de Dropdowns
        private void CargarListasUI(Usuario? model = null)
        {
            // Carga de Roles desde la base de datos
            ViewBag.RolId = new SelectList(_context.Rol, "Id", "NombreRol", model?.RolId);

            var listaGeneros = _context.Genero.ToList();
            ViewBag.Generos = listaGeneros;

            // Carga de Estados estáticos
            ViewBag.StatusList = new SelectList(new[] {
                new { Text = "Activo", Value = "A" },
                new { Text = "Inactivo", Value = "I" },
                new { Text = "Baja", Value = "B" }
            }, "Value", "Text", model?.Status.ToString());

        }

        private static IEnumerable<SelectListItem> StatusOpcionesUsuario() => new[]
{
            new SelectListItem("Activo","A"),
            new SelectListItem("Inactivo","I"),
            new SelectListItem("Baja","B")
        };
        // GET: Usuarios
        public async Task<IActionResult> Index(
            [Bind(Prefix = "Filtro")] UsuarioFilterViewModel filtro,
            string? mode)
        {

            filtro.StatusOpciones = StatusOpcionesUsuario();
            filtro.Page = filtro.Page <= 0 ? 1 : filtro.Page;
            filtro.PageSize = 10;


            IQueryable<Usuario> q = _context.Usuarios.AsNoTracking();

            bool aplicarFiltro =
           mode == "filter" ||
           (!filtro.MostrarTodos &&
            (filtro.Id.HasValue ||
             !string.IsNullOrWhiteSpace(filtro.Nombre) ||
             filtro.FechaAltaDesde.HasValue ||
             filtro.FechaAltaHasta.HasValue ||
             (filtro.StatusSeleccionados?.Any() ?? false)));


            if (aplicarFiltro)
            {
                if (filtro.Id.HasValue)
                    q = q.Where(c => c.Id == filtro.Id.Value);

                if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                    q = q.Where(c => EF.Functions.Like(c.Nombre, $"%{filtro.Nombre}%"));

                if (filtro.FechaAltaDesde.HasValue)
                    q = q.Where(c => c.FechaAlta >= filtro.FechaAltaDesde.Value.Date);

                if (filtro.FechaAltaHasta.HasValue)
                {
                    var hasta = filtro.FechaAltaHasta.Value.Date.AddDays(1).AddTicks(-1);
                    q = q.Where(c => c.FechaAlta <= hasta);
                }

                if ((filtro.StatusSeleccionados != null) && (filtro.StatusSeleccionados.Any()))
                {
                    var set = filtro.StatusSeleccionados.Select(s => s[0]).ToHashSet();
                    q = q.Where(c => set.Contains(c.Status));
                }
            }


            q = q.OrderBy(c => c.Id);
            var paged = await PaginatedList<Usuario>.CreateAsync(q, filtro.Page, filtro.PageSize);
            var vm = new UsuarioIndexViewModel { Filtro = filtro, Resultados = paged };
            return View(vm);

            //var appDBContext = _context.Usuarios.Include(u => u.Rol);
            //return View(await appDBContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {

            CargarListasUI();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,ApellidoMaterno,ApellidoPaterno,Email,Pass,Status,FechaNacimiento,RolId,GeneroId, FotoRuta")] Usuario usuario, IFormFile fotoUsuario)
        {
            ModelState.Remove("FotoRuta");
            if (fotoUsuario != null)
            {
             
                string carpetaDestino = Path.Combine(_hostEnvironment.WebRootPath, "Imagenes", "Usuarios");

             
                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

            
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fotoUsuario.FileName);
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);


                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await fotoUsuario.CopyToAsync(stream);
                }
          
                usuario.FotoRuta = "/Imagenes/Usuarios/" + nombreArchivo;
            }

            if (ModelState.IsValid)
            {            
         
                usuario.Salt = Seguridad.GenerarSalt();
                usuario.Pass = Seguridad.GenerarHash(usuario.Pass, usuario.Salt);
                usuario.FechaAlta = DateTime.Now;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            CargarListasUI(usuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();


            CargarListasUI(usuario);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,ApellidoMaterno,ApellidoPaterno,Email,Status,FechaNacimiento,RolId,GeneroId, FotoRuta")] Usuario usuario, IFormFile fotoUsuario)
        {
            if (id != usuario.Id) return NotFound();

            var usuarioAntiguo = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            //ModelState.Remove("FotoRuta");
            if (fotoUsuario != null)
            {
                // 1. Ruta local: inventario/wwwroot/imagenes/Usuarios
                string carpetaDestino = Path.Combine(_hostEnvironment.WebRootPath, "Imagenes", "Usuarios");

                // 2. Crear carpeta si no existe en tu PC
                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                // 3. Nombre de archivo único (para evitar choques en local)
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fotoUsuario.FileName);
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                // 4. Guardar físicamente el archivo
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await fotoUsuario.CopyToAsync(stream);
                }
                // 5. Guardamos la ruta relativa para la DB (lo que usará el navegador)
                usuario.FotoRuta = "/Imagenes/Usuarios/" + nombreArchivo;
            }
            else 
            {
                usuario.FotoRuta = usuarioAntiguo.FotoRuta;
            }

            usuario.Pass = usuarioAntiguo.Pass;
            ModelState.Remove("Pass");
            ModelState.Remove("Rol");
            ModelState.Remove("FotoRuta");
            ModelState.Remove("fotoUsuario");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Attach(usuario);
                    _context.Entry(usuario).State = EntityState.Modified;
                    _context.Entry(usuario).Property(x => x.Salt).IsModified = false;
                    _context.Entry(usuario).Property(x => x.Pass).IsModified = false;
                 
                  
                    await _context.SaveChangesAsync();
                    TempData["MensajeExito"] = "El cambio fue exitoso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id)) return NotFound();
                    else throw;
                }
               
            }

            CargarListasUI(usuario);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
  

            if (usuario == null) return NotFound();


            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Status = 'B';
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}