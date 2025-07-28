using Microsoft.AspNetCore.Identity;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Utilities;

namespace WareHouseSTARNET.UserServices
{
    public class SeedService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            string email = "admin@dmin.cz";
            string password = "12345678";

            var adminUser = await _userManager.FindByEmailAsync(email);
            if(adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateTime(1996, 8, 21)
                };

                var result = await _userManager.CreateAsync(newAdmin, password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdmin, Roles.Admin);
                }
            }
        }
    }
}
