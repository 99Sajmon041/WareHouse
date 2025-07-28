using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;

namespace WareHouseSTARNET.Services.Implementations
{
    public class FormHelperService : IFormHelperService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ITypeOfMaterialRepository _typeOfMaterialRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormHelperService(IMaterialRepository materialRepository,
            ITypeOfMaterialRepository typeOfMaterialRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _materialRepository = materialRepository;
            _typeOfMaterialRepository = typeOfMaterialRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SelectListItem>> GetMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            return materials.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetTypeOfMaterialAsync()
        {
            var typeOfMaterial = await _typeOfMaterialRepository.GetAllAsync();
            return typeOfMaterial.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Type
            });
        }

        public Task<IEnumerable<SelectListItem>> GetRolesAsync()
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = Roles.Admin, Text = "Administrátor" },
                new SelectListItem { Value = Roles.User, Text = "Technik" }
            };
            return Task.FromResult(roles.AsEnumerable());
        }

        public async Task<IEnumerable<SelectListItem>> GetApplicationUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new SelectListItem
            {
                Text = $"{u.FirstName} {u.LastName}",
                Value = u.Id
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetUsersByRoleAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (user == null)
            {
                throw new EntityNotFoundException("Aktuální uživatel nebyl nalezen.");
            }

            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                var allUsers = await _userManager.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = $"{u.FirstName} {u.LastName}"
                    })
                    .ToListAsync();

                return allUsers
                    .GroupBy(u => u.Text)
                    .Select(g => g.First())
                    .OrderBy(x => x.Text)
                    .ToList();
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                         Value = user.Id,
                         Text = $"{user.FirstName} {user.LastName}"
                     }
                };
            }
        }
    }
}
