using WareHouseSTARNET.Models;
using WareHouseSTARNET.ViewModels.DashboardViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IDashboardService
    {
        List<DashboardTechniciansViewModel> GetTechnicianStats(DateTime? from, DateTime? to);
        List<DashboardMaterialsViewModel> GetMaterialStats(DateTime? from, DateTime? to);
        TechnicianStatsViewModel GetExtendedTechnicianStats(string technicianId, DateTime? from, DateTime? to);
        Task<MaterialStatsViewModel> GetExtendedMaterialStats(int materialId, DateTime? from, DateTime? to);
        Task<List<CriticalMaterialViewModel>> GetCriticalMaterialsAsync();
        List<MaterialChartItem> GetMaterialWrittenOffChart(string period);
        List<TechnicianChartItem> GetTechnicianWrittenOffChart(string period);
    }
}
