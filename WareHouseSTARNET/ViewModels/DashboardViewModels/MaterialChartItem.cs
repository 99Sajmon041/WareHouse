namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class MaterialChartItem
    {
        public string MaterialName { get; set; } = null!;
        public int WrittenOffCount { get; set; }
    }

    public class MaterialChartViewModel
    {
        public List<MaterialChartItem> ChartData { get; set; } = new();
        public string SelectedPeriod { get; set; } = "month";
    }
}