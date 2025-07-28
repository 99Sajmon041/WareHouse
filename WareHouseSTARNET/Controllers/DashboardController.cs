using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.Utilities;
using WareHouseSTARNET.ViewModels.DashboardViewModels;

namespace WareHouseSTARNET.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }


        [HttpGet]
        public IActionResult Technicians(DateTime? from, DateTime? to, string period = "month")
        {
            var topTechnicians = _dashboardService.GetTechnicianStats(from, to);
            var chartData = _dashboardService.GetTechnicianWrittenOffChart(period);

            var ViewModel = new DashboardTechniciansPageViewModel
            {
                TopTechnicinas = topTechnicians,
                ChartViewModel = new TechnicianChartViewModel
                {
                    ChartData = chartData,
                    SelectedPeriod = period,
                }
            };
            return View(ViewModel);
        }


        [HttpGet]
        public IActionResult Materials(DateTime? tableFrom, DateTime? tableTo, string period = "month")
        {
            var topMaterials = _dashboardService.GetMaterialStats(tableFrom, tableTo);
            var chartData = _dashboardService.GetMaterialWrittenOffChart(period);

            var viewModel = new DashboardMaterialsPageViewModel
            {
                TopMaterials = topMaterials,
                ChartViewModel = new MaterialChartViewModel
                {
                    SelectedPeriod = period,
                    ChartData = chartData
                }
            };
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult TechnicianDetails(string id, DateTime? from, DateTime? to)
        {
            try
            {
                var individualTechnicianStats = _dashboardService.GetExtendedTechnicianStats(id, from, to);
                return View(individualTechnicianStats);
            }
            catch (EntityNotFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new TechnicianStatsViewModel
                {
                    TechnicianName = "",
                    DateFrom = from ?? DateTime.Now.AddMonths(-1),
                    DateTo = to ?? DateTime.Now,
                    MaterialStats = new List<TechnicianStatsViewModel.MaterialStat>()
                });
            }
        }



        [HttpGet]
        public async Task<IActionResult> MaterialDetails(int id, DateTime? from, DateTime? to)
        {
            try
            {
                var stats = await _dashboardService.GetExtendedMaterialStats(id, from, to);
                return View(stats);
            }
            catch (EntityNotFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new MaterialStatsViewModel
                {
                    DateFrom = from ?? DateTime.Now.AddMonths(-1),
                    DateTo = to ?? DateTime.Now
                });
            }
        }
    }
}
