using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels;

namespace WareHouseSTARNET.Controllers
{
    public class WrittenOffMaterialController : Controller
    {
        private readonly IWrittenOffMaterialService _writtenOffMaterialService;
        private readonly IFormHelperService _formHelperService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WrittenOffMaterialController(IWrittenOffMaterialService writtenOffMaterialService,
            IFormHelperService formHelperService,
            UserManager<ApplicationUser> userManager)
        {
            _writtenOffMaterialService = writtenOffMaterialService;
            _formHelperService = formHelperService;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index(DateTime? date, int page = 1, int pageSize = 20)
        {
            IEnumerable<WrittenOffMaterialViewModel> materials;

            if (date.HasValue)
            {
                materials = await _writtenOffMaterialService.GetAllWithDetailsAsync(date.Value);
                ViewBag.Date = date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                materials = await _writtenOffMaterialService.GetAllWrittenMaterialsAsync();
            }

            var totalitems = materials.Count();
            var paginatedMaterials = materials
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalitems / pageSize);

            return View(paginatedMaterials);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _writtenOffMaterialService.DeleteWrittenMaterialAsync(id);
                TempData["SuccessMessage"] = $"Materiál s ID: {id} byl úspěšně smazán.";
                return RedirectToAction(nameof(Index));
            }
            catch(EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Create()
        {
            var materials = await _formHelperService.GetMaterialsAsync();
            if (!materials.Any())
            {
                TempData["ErrorMessage"] = "Není dostupný žádný materiál, nejdříve jej vytvořte!";
                return RedirectToAction("Create", "Material");
            }
            IEnumerable<SelectListItem> users;
            ApplicationUser? currentUser = null;
            try
            {
                users = await _formHelperService.GetUsersByRoleAsync();
                currentUser = await _userManager.GetUserAsync(User);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole(Roles.User) && currentUser == null)
            {
                TempData["ErrorMessage"] = "Nepodařilo se načíst aktuálního uživatele.";
                return RedirectToAction("Index", "Home");
            }
            string? applicationUserId = null;
            if (User.IsInRole(Roles.User))
            {
                applicationUserId = currentUser!.Id;
            }
            var createModel = new WrittenOffMaterialCreateViewModel
            {
                WrittenOffDate = DateTime.Today,
                Materials = materials,
                ApplicationUsers = users,
                ApplicationUserId = applicationUserId!
            };
            return View(createModel);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Create(WrittenOffMaterialCreateViewModel createModel)
        {
            if(!ModelState.IsValid)
            {
                createModel.Materials = await _formHelperService.GetMaterialsAsync();
                createModel.ApplicationUsers = await _formHelperService.GetUsersByRoleAsync();
                return View(createModel);
            }
            try
            {
                await _writtenOffMaterialService.CreateWrittenMaterialAsync(createModel);
                TempData["SuccessMessage"] = "Materiál byl úspěšně odepsán.";
                if(User.IsInRole(Roles.Admin))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["SuccessMessage"] = "Materiál byl úspěšně odepsán, můžete odepsat další.";
                    return RedirectToAction(nameof(Create));
                }
            }
            catch(EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch(ForbiddenOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                createModel.Materials = await _formHelperService.GetMaterialsAsync();
                createModel.ApplicationUsers = await _formHelperService.GetUsersByRoleAsync();
                return View(createModel);
            }
        }
    }
}
