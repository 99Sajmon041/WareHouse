using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.UserViewModels;

namespace WareHouseSTARNET.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IFormHelperService _formHelperService;

        public ApplicationUserController(IApplicationUserService applicationUserService, IFormHelperService formHelperService)
        {
            _applicationUserService = applicationUserService;
            _formHelperService = formHelperService;
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var users = await _applicationUserService.GetAllAsync();
            if(!users.Any())
            {
                TempData["ErrorMessage"] = "Žádní uživatelé nebyli nalezeni, nejprve musíte vytvořit!";
                return RedirectToAction("Create");
            }
            return View(users);
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Details(string id)
        {
            var currentUserId = await _applicationUserService.GetCurrentUserIdAsync(User);
            if(User.IsInRole(Roles.User) && id != currentUserId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            try
            {
                var user = await _applicationUserService.GetByIdAsync(id);
                return View(user);
            }
            catch(EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _applicationUserService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Uživatel byl odstraněn";
                return RedirectToAction(nameof(Index));
            }
            catch(ForbiddenOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var updateModel = await _applicationUserService.GetUpdateViewModelAsync(id);
                return View(updateModel);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(ApplicationUserUpdateViewModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                updateModel.Roles = await _formHelperService.GetRolesAsync();
                return View(updateModel);
            }
            try
            {
                await _applicationUserService.UpdateAsync(updateModel);
                TempData["SuccessMessage"] = $"Uživatel: {updateModel.FirstName} {updateModel.LastName} byl úspěšně upraven!";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var createUserModel = new ApplicationUserCreateViewModel
            {
                Roles = await _formHelperService.GetRolesAsync(),
                DateOfBirth = DateTime.Now,
            };
            return View(createUserModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(ApplicationUserCreateViewModel createModel)
        {
            if(!ModelState.IsValid)
            {
                createModel.Roles = await _formHelperService.GetRolesAsync();
                return View(createModel);
            }
            try
            { 
                await _applicationUserService.CreateAsync(createModel);
                TempData["SuccessMessage"] = $"Uživatel: {createModel.FirstName} {createModel.LastName} byl úspěšně vytvořen!";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Vytvoření uživatele selhalo: {ex.Message}.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
