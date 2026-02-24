using inventario.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using inventario.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace inventario.Controllers
{
    public class ProductoIngredienteController : Controller
    {

        private readonly AppDBContext _context;
        private readonly string _connectionString;

  
        public ProductoIngredienteController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
          
            _connectionString = configuration.GetConnectionString("AzureStorage");
        }
 
        public async Task<IActionResult> Index()
        {
            var ingredientes = await _context.Ingredientes.ToListAsync();
            return View(ingredientes ?? new List<ProductoIngrediente>()); 
        }


        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoIngrediente ingrediente, IFormFile fotoArchivo)
        {

            ModelState.Remove(nameof(ingrediente.UrlImagen));

            if (ModelState.IsValid)
            {
                if (fotoArchivo != null && fotoArchivo.Length > 0)
                {
                    try
                    {
                        var blobServiceClient = new BlobServiceClient(_connectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient("ingredientes");


                        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                        string fileName = $"{Guid.NewGuid()}.jpg";
                        var blobClient = containerClient.GetBlobClient(fileName);

                        using (var inputStream = fotoArchivo.OpenReadStream())
                        {
                            using (var image = await Image.LoadAsync(inputStream))
                            {
               
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(600, 0)
                                }));

                                using (var outputStream = new MemoryStream())
                                {

                                    await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 75 });
                                    outputStream.Position = 0;

                                    await blobClient.UploadAsync(outputStream, true);
                                }
                            }
                        }

               
                        ingrediente.UrlImagen = blobClient.Uri.ToString();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error al procesar la imagen: " + ex.Message);
                        return View(ingrediente);
                    }
                }

     
                _context.Add(ingrediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ingrediente);
        }

 
        public ActionResult Edit(int id)
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoIngredienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoIngredienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
