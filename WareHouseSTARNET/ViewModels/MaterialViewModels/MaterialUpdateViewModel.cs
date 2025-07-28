using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.MaterialViewModels
{
    public class MaterialUpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Název materiálu")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Zadejte název v rozmezí 5 - 50 znaků")]
        public string Name { get; set; } = null!;


        [Required]
        [Display(Name = "Popis materiálu")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Zadejte popis v rozmezí 10 - 100 znaků")]
        public string Description { get; set; } = null!;


        [Required]
        [Display(Name = "Množství skladem")]
        [Range(0, 200, ErrorMessage = "Zadejte množství položek skladem v rozmezí 0 - 200!")]
        public int Quantity { get; set; }


        [Required]
        [Display(Name = "Krytické množství skladem")]
        [Range(1, 100, ErrorMessage = "Zadejte minimální počet položek skladem v rozmezí 1 - 100!")]
        public int CriticalQuantity { get; set; }


        [Required]
        [Display(Name = "Cena za kus")]
        [Range(5, 10000000, ErrorMessage = "Cena musí být v rozmezí 5 až 1 000 000 Kč.")]
        public decimal UnitPrice { get; set; }


        [Display(Name = "Nový obrázek (nepovinný)")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }


        [Required(ErrorMessage = "Vyberte typ materiálu.")]
        [Display(Name = "Typ materilálu")]
        public int TypeOfMaterialId { get; set; }


        public IEnumerable<SelectListItem> TypeOfMaterials { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
