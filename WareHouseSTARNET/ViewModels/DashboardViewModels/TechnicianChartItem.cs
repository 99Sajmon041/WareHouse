namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class TechnicianChartItem
    {
        public string TechnicianName { get; set; } = null!;
        public int WrittenOffCount { get; set; }
    }

    public class TechnicianChartViewModel
    {
        public List<TechnicianChartItem> ChartData { get; set; } = new();
        public string SelectedPeriod { get; set; } = "month";
    }
}
