using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels;

namespace WareHouseSTARNET.Controllers
{
    public class TypeOfMaterialController : Controller
    {
        private readonly ITypeOfMaterialService _typeOfMaterialService;
        private readonly IMapper _mapper;

        public TypeOfMaterialController(ITypeOfMaterialService typeOfMaterialService, IMapper mapper)
        {
            _typeOfMaterialService = typeOfMaterialService;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Index()
        {
            var types = await _typeOfMaterialService.GetAllTypeOfMaterialsAsync();
            if (!types.Any())
            {
                TempData["ErrorMessage"] = "Zatím nebyly přidány žádné typy materiálů.";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                return View(types);
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin + ", " + Roles.User)]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var type = await _typeOfMaterialService.GetTypeOfMaterialByIdAsync(id);
                return View(type);
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
                await _typeOfMaterialService.DeleteTypeOfMaterialAsync(id);
                TempData["SuccessMessage"] = $"Typ materiálu s ID:{id} byl úspěšně smazán.";
                return RedirectToAction(nameof(Index));
            }
            catch(EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var type = await _typeOfMaterialService.GetTypeOfMaterialByIdAsync(id);
                var updateType = _mapper.Map<TypeOfMaterialUpdateViewModel>(type);
                return View(updateType);
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
        public async Task<IActionResult> Edit(TypeOfMaterialUpdateViewModel updateModel)
        {
            if(!ModelState.IsValid)
            {
                return View(updateModel);
            }
            try
            {
                await _typeOfMaterialService.UpdateTypeOfMaterialAsync(updateModel);
                TempData["SuccessMessage"] = $"Typ materiálu s ID: {updateModel.Id} byl úspěšně upraven.";
                return RedirectToAction(nameof(Index));
            }
            catch(EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(TypeOfMaterialCreateViewModel createModel)
        {
            if(!ModelState.IsValid)
            {
                return View(createModel);
            }
            try
            {
                await _typeOfMaterialService.CreateTypeOfMaterialAsync(createModel);
                TempData["SuccessMessage"] = $"Typ: {createModel.Type} byl úspěšně vytvořen";
                return RedirectToAction(nameof(Index));
            }
            catch(EntityAlreadyExistsException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(createModel);
            }
        }
    }
}
