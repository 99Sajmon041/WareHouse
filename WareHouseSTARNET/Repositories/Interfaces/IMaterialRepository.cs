using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.Repositories.Interfaces
{
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        Task<Material?> GetByNameAsync(string name);
        Task<IEnumerable<Material>> GetAllWithTypeAsync();
        Task<Material?> GetWithTypeAsync(int id);
        Task<int> GetQuantityByMaterialIdAsync(int id);
    }
}
