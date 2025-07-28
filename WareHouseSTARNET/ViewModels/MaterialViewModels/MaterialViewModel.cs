using System.ComponentModel.DataAnnotations.Schema;
using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.ViewModels.MaterialViewModels
{
    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public int CriticalQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string TypeOfMaterialName { get; set; } = null!;
        public int TypeOfMaterialId { get; set; }
    }
}
