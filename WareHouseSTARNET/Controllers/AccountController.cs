using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.AccountViewModel;

namespace WareHouseSTARNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if(user == null)
            {
                ModelState.AddModelError(nameof(loginModel.Password), "Neplatné přihlašovací údaje!");
                return View(loginModel);
            }
            var result = await _signInManager.PasswordSignInAsync(user,loginModel.Password, loginModel.RememberMe, false);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(nameof(loginModel.Password), "Neplatné přihlašovací údaje!");
            return View(loginModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Profile()
        {
            var id = await _applicationUserService.GetCurrentUserIdAsync(User);
            if(id == null)
            {
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction("Details", "ApplicationUser", new { id });
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Uživatel nebyl nalezen.";
                return RedirectToAction(nameof(Login));
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Heslo bylo úspěšně změněno.";
                return RedirectToAction(nameof(Profile));
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
