using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.MaterialViewModels
{
    public class MaterialAddToStockViewModel
    {
        public int Id { get; set; }
        public string MaterialName { get; set; } = null!;


        [Range(0, int.MaxValue, ErrorMessage = "Zadejte kladnou hodnotu")]
        public int QuantityToAdd { get; set; } 
    }
}
