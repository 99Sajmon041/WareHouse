using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels
{
    public class TypeOfMaterialViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Typ materiálu")]
        public string Type { get; set; } = null!;
    }
}
