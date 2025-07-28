using WareHouseSTARNET.ViewModels;
using WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface ITypeOfMaterialService
    {
        public Task CreateTypeOfMaterialAsync(TypeOfMaterialCreateViewModel createModel);
        public Task DeleteTypeOfMaterialAsync(int id);
        public Task<IEnumerable<TypeOfMaterialViewModel>> GetAllTypeOfMaterialsAsync();
        public Task<TypeOfMaterialViewModel> GetTypeOfMaterialByIdAsync(int id);
        public Task UpdateTypeOfMaterialAsync(TypeOfMaterialUpdateViewModel updateModel);
    }
}
