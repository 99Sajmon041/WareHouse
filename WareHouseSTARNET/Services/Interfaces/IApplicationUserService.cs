using System.Security.Claims;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.ViewModels.UserViewModels;

namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUserViewModel>> GetAllAsync();
        Task<ApplicationUserViewModel> GetByIdAsync(string id);
        Task UpdateAsync(ApplicationUserUpdateViewModel updateUserModel);   
        Task<ApplicationUserUpdateViewModel> GetUpdateViewModelAsync(string id);
        Task CreateAsync(ApplicationUserCreateViewModel createUserModel);
        Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user);
        Task DeleteAsync(string id);
    }
}
