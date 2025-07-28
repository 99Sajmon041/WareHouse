using System.ComponentModel.DataAnnotations;
using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class DashboardTechniciansViewModel
    {
        [Display(Name = "Technik")]
        public string TechnicianName { get; set; } = null!;

        public string TechnicianId { get; set; } = null!;


        [Display(Name = "Počet odepsáného materiálu")]
        public int WrittenMaterialCount { get; set; }


        [Display(Name = "Datum od:")]
        public DateTime DateFrom { get; set; }


        [Display(Name = "Datum do:")]
        public DateTime DateTo { get; set; }
    }
}
