namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class DashboardMaterialsPageViewModel
    {
        public List<DashboardMaterialsViewModel> TopMaterials { get; set; } = new();
        public MaterialChartViewModel ChartViewModel { get; set; } = new();
    }
}
