using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;

namespace WareHouseSTARNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IEmailService _emailService;
        public HomeController(IDashboardService dashboardService, IEmailService emailService)
        {
            _dashboardService = dashboardService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var criticalMaterials = await _dashboardService.GetCriticalMaterialsAsync();
            return View(criticalMaterials);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> SendCriticalStockEmail()
        {
            var criticalMaterials = await _dashboardService.GetCriticalMaterialsAsync();
            if(!criticalMaterials.Any())
            {
                TempData["ErrorMessage"] = "Žádný materiál nemá kritické množství.";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                await _emailService.SendCriticalStockEmailAsync(criticalMaterials);
                TempData["SuccessMessage"] = "E-mail byl úspìšnì odeslán.";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Nastala chyba pøi odesílání e-mailu: " + ex.Message + " Kontaktujte správce systému.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
