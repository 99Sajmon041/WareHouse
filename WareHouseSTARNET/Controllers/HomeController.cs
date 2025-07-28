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
                TempData["ErrorMessage"] = "��dn� materi�l nem� kritick� mno�stv�.";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                await _emailService.SendCriticalStockEmailAsync(criticalMaterials);
                TempData["SuccessMessage"] = "E-mail byl �sp�n� odesl�n.";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Nastala chyba p�i odes�l�n� e-mailu: " + ex.Message + " Kontaktujte spr�vce syst�mu.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
