using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.Repositories.Interfaces
{
    public interface ITypeOfMaterialRepository : IGenericRepository<TypeOfMaterial>
    {
        Task<TypeOfMaterial?> GetTypeByIdAsync(int id);
        Task<TypeOfMaterial?> GetTypeByNameAsync(string type);
    }
}
