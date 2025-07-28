using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels
{
    public class TypeOfMaterialUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Typ materiálu je povinný.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Zadejte typ materiálu v rozmezí 5 - 50 znaků!")]


        [Display(Name = "Typ materiálu")]
        public string Type { get; set; } = null!;
    }
}
