using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventario.Models.ViewModels
{
    public class UsuarioFilterViewModel
    {
        
        public int? Id { get; set; }
        
        [DisplayName("Patrón de nombre")]
        public string? Nombre { get; set; }


        [Display(Name = "Alta desde"), DataType(DataType.Date)]
        public DateTime? FechaAltaDesde { get; set; }
        [Display(Name = "Alta hasta"), DataType(DataType.Date)]
        public DateTime? FechaAltaHasta { get; set; }
        public List<string> StatusSeleccionados { get; set; } = new();

        public IEnumerable<SelectListItem> StatusOpciones { get; set; } =
                 new List<SelectListItem>();

        public bool MostrarTodos { get; set; }

        //Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
