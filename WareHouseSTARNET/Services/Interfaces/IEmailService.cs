using WareHouseSTARNET.ViewModels.DashboardViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendCriticalStockEmailAsync(List<CriticalMaterialViewModel> criticalMaterials);
    }
}
