using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class CriticalMaterialViewModel
    {
        [Display(Name = "Název materiálu")]
        public string MaterialName { get; set; } = null!;


        [Display(Name = "Kritické množství")]
        public int CriticalQuantity { get; set; }


        [Display(Name = "Množství na skladě")]
        public int Quantity { get; set; }
    }
}
