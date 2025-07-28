using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.Repositories.Interfaces
{
    public interface IWrittenOffMaterialRepository : IGenericRepository<WrittenOffMaterial>
    {
        Task<IEnumerable<WrittenOffMaterial>> GetAllWithDetailsAsync(DateTime? date = null);
        Task<int> GetMonthlyWrittenOffAsync(int materialId, string userId);
    }
}
