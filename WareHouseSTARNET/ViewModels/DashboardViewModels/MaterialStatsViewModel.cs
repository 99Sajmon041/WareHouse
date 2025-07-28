using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class MaterialStatsViewModel
    {
        [Display(Name = "Datum od:")]
        public DateTime DateFrom { get; set; }


        [Display(Name = "Datum do:")]
        public DateTime DateTo { get; set; }


        [Display(Name = "Název materiálu")]
        public string MaterialName { get; set; } = null!;


        [Display(Name = "Typ materiálu")]
        public string? TypeOfMaterialName { get; set; }


        [Display(Name = "Počet odepsaných kusů")]
        public ushort QuantityOfWrittenPieces { get; set; }


        [Display(Name = "Počet kusů skladem")]
        public ushort StockInPieces { get; set; }


        [Display(Name = "Odepsání za poslední týden")]
        public ushort LastWeekConsumption { get; set; }


        [Display(Name = "Odepsání za poslední měsíc")]
        public ushort LastMonthConsumption { get; set; }


        [Display(Name = "Odepsání za poslední čtvrt roku")]
        public ushort LastQuaterOfYearConsumption { get; set; }


        [Display(Name = "Odepsání za poslední rok")]
        public ushort LastYearConsumption { get; set; }

        public bool IsStateCritical { get; set; }
    }
}
