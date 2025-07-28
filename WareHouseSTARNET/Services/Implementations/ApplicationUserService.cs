using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.UserViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IFormHelperService _formHelperService;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IFormHelperService formHelperService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _formHelperService = formHelperService;
        }

        public async Task DeleteAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var actualUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (appUser!.Id == actualUser!.Id)
            {
                throw new ForbiddenOperationException($"Uživatel: {actualUser.FullName} nemůže být odstraněn, je aktuálně přihlášený!");
            }
            await _userManager.DeleteAsync(appUser);
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAllAsync()
        {
            var appUsers = await _userManager.Users.ToListAsync();
            var userViewModels = _mapper.Map<List<ApplicationUserViewModel>>(appUsers);

            for (int i = 0; i < appUsers.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(appUsers[i]);
                userViewModels[i].Role = roles.FirstOrDefault() ?? "N/A";
            }
            return userViewModels;
        }

        public async Task<ApplicationUserViewModel> GetByIdAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                throw new EntityNotFoundException($"Uživatel s ID: {id} nebyl nalezen!");
            }
            var userModel = _mapper.Map<ApplicationUserViewModel>(appUser);
            var role = await _userManager.GetRolesAsync(appUser);
            userModel.Role = role.FirstOrDefault() ?? "N/A";
            return userModel;
        }

        public async Task CreateAsync(ApplicationUserCreateViewModel createUserModel)
        {
            var appUser = _mapper.Map<ApplicationUser>(createUserModel);
            appUser.UserName = createUserModel.Email;
            var result = await _userManager.CreateAsync(appUser, createUserModel.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Vytvoření uživatele selhalo: {errors}.");
            }
            var roleResult = await _userManager.AddToRoleAsync(appUser, createUserModel.Role);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Vytvoření role selhalo: {errors}.");
            }
        }

        public async Task UpdateAsync(ApplicationUserUpdateViewModel updateUserModel)
        {
            var appUser = await _userManager.FindByIdAsync(updateUserModel.Id);
            if (appUser == null)
            {
                throw new EntityNotFoundException($"Uživatel s ID: {updateUserModel.Id} nebyl nalezen.");
            }
            appUser.FirstName = updateUserModel.FirstName;
            appUser.LastName = updateUserModel.LastName;
            appUser.PhoneNumber = updateUserModel.PhoneNumber;
            appUser.DateOfBirth = updateUserModel.DateOfBirth;

            if (appUser.Email != updateUserModel.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(appUser, updateUserModel.Email);
                if (!setEmailResult.Succeeded)
                {
                    var errors = string.Join(", ", setEmailResult.Errors.Select(e => e.Description));
                    throw new Exception($"Změna e-mailu selhala: {errors}");
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(appUser, updateUserModel.Email);
                if (!setUserNameResult.Succeeded)
                {
                    var errors = string.Join(", ", setUserNameResult.Errors.Select(e => e.Description));
                    throw new Exception($"Změna uživatelského jména selhala: {errors}");
                }
            }
            var result = await _userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Aktualizace uživatele selhala: {errors}");
            }
            var currentRoles = await _userManager.GetRolesAsync(appUser);
            if (!currentRoles.Contains(updateUserModel.Role))
            {
                await _userManager.RemoveFromRolesAsync(appUser, currentRoles);
                await _userManager.AddToRoleAsync(appUser, updateUserModel.Role);
            }
        }

        public async Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            return appUser?.Id;
        }

        public async Task<ApplicationUserUpdateViewModel> GetUpdateViewModelAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException($"Uživatel s ID: {id} nebyl nalezen.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            return new ApplicationUserUpdateViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                DateOfBirth = user.DateOfBirth,
                Role = roles.FirstOrDefault() ?? "N/A",
                Roles = await _formHelperService.GetRolesAsync()
            };
        }
    }
}
