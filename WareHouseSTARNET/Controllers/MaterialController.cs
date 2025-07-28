using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.MaterialViewModels;

namespace WareHouseSTARNET.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        private readonly IFormHelperService _formHelperService;

        public MaterialController(IMaterialService materialService, IFormHelperService formHelperService)
        {
            _materialService = materialService;
            _formHelperService = formHelperService;
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var materials = await _materialService.GetAllMaterialsAsync();
                return View(materials);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                if(User.IsInRole(Roles.Admin))
                {
                    return RedirectToAction(nameof(Create), "Material");
                }
                else
                {
                    return RedirectToAction(nameof(Index), "Home");
                }
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var material = await _materialService.GetMaterialByIdAsync(id);
                return View(material);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _materialService.DeleteMaterialAsync(id);
                TempData["SuccessMessage"] = $"Materiál s ID: {id} byl odstraněn";
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var updateViewModel = await _materialService.GetMaterialUpdateViewModelByIdAsync(id);
                return View(updateViewModel);
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
        public async Task<IActionResult> Edit(MaterialUpdateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.TypeOfMaterials = await _formHelperService.GetTypeOfMaterialAsync();
                return View(model);
            }
            try
            {
                await _materialService.UpdateMaterialAsync(model);
                TempData["SuccessMessage"] = $"Materiál s ID: {model.Id} byl upraven";
                return RedirectToAction(nameof(Index));
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var types = await _formHelperService.GetTypeOfMaterialAsync();
            if(!types.Any())
            {
                TempData["ErrorMessage"] = "nejsou žádné typy, nejdříve je vytvořte!";
                return RedirectToAction("Create", "TypeOfMaterial");
            }
            return View(new MaterialCreateViewModel
            {
                TypeOfMaterials = types
            });
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(MaterialCreateViewModel createModel)
        {
            if(!ModelState.IsValid)
            {
                createModel.TypeOfMaterials = await _formHelperService.GetTypeOfMaterialAsync();
                return View(createModel);
            }

            try
            {
                await _materialService.CreateMaterialAsync(createModel);
                TempData["SuccessMessage"] = $"Materiál: {createModel.Name} byl úspěšně vytvořen!";
                return RedirectToAction(nameof(Index));
            }
            catch (EntityAlreadyExistsException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(createModel);
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddToStock()
        {
            var materials = await _materialService.GetAllMaterialsAsync();
            if (!materials.Any())
            {
                TempData["ErrorMessage"] = "Nejsou žádné materiály, nejdříve je vytvořte!";
                return RedirectToAction(nameof(Create));
            }
            var model = materials.Select(m => new MaterialAddToStockViewModel
            {
                Id = m.Id,
                MaterialName = m.Name,
                QuantityToAdd = 0
            }).ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddToStock(List<MaterialAddToStockViewModel> model)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Některé položky nejsou platné.";
                return View(model);
            }
            int countOfEdited = 0;
            foreach(var item in model)
            {
                if(item.QuantityToAdd > 0)
                {
                    countOfEdited++;
                }
                await _materialService.AddQuantityAsync(item.Id, item.QuantityToAdd);
            }
            if(countOfEdited == 0)
            {
                TempData["ErrorMessage"] = "Žádné položky nebyly naskladněny, zadejte množství.";
                return View(model);
            }
            TempData["SuccessMessage"] = $"Celkem bylo naskladněno u: {countOfEdited} položek.";
            return RedirectToAction(nameof(Index));
        }
    }
}
