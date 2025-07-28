using WareHouseSTARNET.Models;
using WareHouseSTARNET.ViewModels;
using WareHouseSTARNET.ViewModels.MaterialViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IMaterialService
    {
        Task CreateMaterialAsync(MaterialCreateViewModel createModel);
        Task DeleteMaterialAsync(int id);
        Task<IEnumerable<MaterialViewModel>> GetAllMaterialsAsync();
        Task<MaterialViewModel?> GetMaterialByIdAsync(int id);
        Task UpdateMaterialAsync(MaterialUpdateViewModel updateModel);
        Task<MaterialUpdateViewModel> GetMaterialUpdateViewModelByIdAsync(int id);
        Task AddQuantityAsync(int materialId, int quantityToAdd);
    }
}
