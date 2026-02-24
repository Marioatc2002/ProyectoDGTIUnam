using inventario.Infrastructure;
using inventario.Models;

namespace inventario.Models.ViewModels
{
    public class UsuarioIndexViewModel
    {

        public UsuarioFilterViewModel Filtro { get; set; } =
         new UsuarioFilterViewModel();

        public PaginatedList<inventario.Models.Usuario>? Resultados { get; set; }

    }

}
