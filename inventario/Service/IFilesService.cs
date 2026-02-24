namespace inventario.Service
{
    public interface IFilesService
    {
        Task<string> SubirArchivo(Stream archivo, string nombre);


    }

}
