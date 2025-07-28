using Microsoft.AspNetCore.Mvc.Rendering;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IFormHelperService
    {
        Task<IEnumerable<SelectListItem>> GetMaterialsAsync();
        Task<IEnumerable<SelectListItem>> GetTypeOfMaterialAsync();
        Task<IEnumerable<SelectListItem>> GetApplicationUsersAsync();
        Task<IEnumerable<SelectListItem>> GetRolesAsync();
        Task<IEnumerable<SelectListItem>> GetUsersByRoleAsync();
    }
}
