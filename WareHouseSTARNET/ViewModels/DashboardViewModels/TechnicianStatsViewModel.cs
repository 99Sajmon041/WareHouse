namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class TechnicianStatsViewModel
    {
        public string TechnicianName { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<MaterialStat> MaterialStats { get; set; } = new();

        public class MaterialStat
        {
            public string MaterialName { get; set; } = null!;
            public int Quantity { get; set; }
        }
    }
}
