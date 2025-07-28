using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels
{
    public class WrittenOffMaterialViewModel
    {
        public int Id { get; set; }
        public DateTime WritteOfDate { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; } = null!;
        public string TypeOfMaterialName { get; set; } = null!;
        public string? ApplicationUser { get; set; }
    }
}
