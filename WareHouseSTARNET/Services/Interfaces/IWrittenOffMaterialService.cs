using WareHouseSTARNET.ViewModels;
using WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IWrittenOffMaterialService
    {
        Task CreateWrittenMaterialAsync(WrittenOffMaterialCreateViewModel createModel);
        Task DeleteWrittenMaterialAsync(int id);
        Task<IEnumerable<WrittenOffMaterialViewModel>> GetAllWrittenMaterialsAsync();
        Task<IEnumerable<WrittenOffMaterialViewModel>> GetAllWithDetailsAsync(DateTime dateTime);
    }
}
