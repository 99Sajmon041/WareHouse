namespace WareHouseSTARNET.ViewModels.DashboardViewModels
{
    public class DashboardTechniciansPageViewModel
    {
        public List<DashboardTechniciansViewModel> TopTechnicinas { get; set; } = new();
        public TechnicianChartViewModel ChartViewModel { get; set; } = new();
    }
}
