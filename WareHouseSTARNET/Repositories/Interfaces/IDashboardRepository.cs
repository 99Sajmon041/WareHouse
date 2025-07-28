using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        IQueryable<WrittenOffMaterial> GetWrittenOffMaterialsBetween(DateTime from, DateTime to);
    }
}
