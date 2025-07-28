using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class DashboardMaterialsViewModel
    {
        [Display(Name = "Materiál")]
        public string MaterialName { get; set; } = null!;

        [Display(Name = "Technik")]
        public string TopTechnicianName { get; set; } = null!;

        [Display(Name = "Odepsáno technikem")]
        public int TopTechnicianQuantity { get; set; }

        public int MaterialId { get; set; }

        [Display(Name = "Typ materiálu")]
        public string MaterialType { get; set; } = null!;

        [Display(Name = "Počet odepsáného materiálu")]
        public int WrittenMaterialCount { get; set; }

        [Display(Name = "Datum od:")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Datum do:")]
        public DateTime DateTo { get; set; }

    }
}
