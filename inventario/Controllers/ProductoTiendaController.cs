using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using inventario.Data;
using inventario.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace inventario.Controllers
{
    public class ProductoTiendaController : Controller
    {
        private readonly AppDBContext _context; 
        private readonly string _connectionString;


        public ProductoTiendaController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
   
            _connectionString = configuration.GetConnectionString("AzureStorage");
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.ProductosTienda.ToListAsync();
            return View(productos);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoTienda producto, IFormFile fotoArchivo)
        {
            // Quitamos la validación de la URL ya que la generamos en el servidor
            ModelState.Remove(nameof(producto.UrlImagen));

            if (ModelState.IsValid)
            {
                if (fotoArchivo != null && fotoArchivo.Length > 0)
                {
                    try
                    {
                        var blobServiceClient = new BlobServiceClient(_connectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient("productos");

                       
                        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                        string extension = ".jpg"; 
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        var blobClient = containerClient.GetBlobClient(fileName);

                        // --- PROCESAMIENTO DE IMAGEN CON IMAGESHARP ---
                        using (var inputStream = fotoArchivo.OpenReadStream())
                        {
                            using (var image = await Image.LoadAsync(inputStream))
                            {
                                
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(800, 0)
                                }));

               
                                using (var outputStream = new MemoryStream())
                                {
                                    var encoder = new JpegEncoder { Quality = 75 }; // Calidad equilibrada
                                    await image.SaveAsJpegAsync(outputStream, encoder);
                                    outputStream.Position = 0; // Reiniciar posición para la subida

                                    await blobClient.UploadAsync(outputStream, true);
                                }
                            }
                        }

                        producto.UrlImagen = blobClient.Uri.ToString();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error al optimizar o subir la imagen: " + ex.Message);
                        return View(producto);
                    }
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

       
    }
}