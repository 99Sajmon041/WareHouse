using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.DashboardViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IWrittenOffMaterialRepository _writtenOffMaterialRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public DashboardService(IDashboardRepository dashboardRepository,
            IMaterialRepository materialRepository,
            IWrittenOffMaterialRepository writtenOffMaterialRepository,
            UserManager<ApplicationUser> userManager)
        {
            _dashboardRepository = dashboardRepository;
            _materialRepository = materialRepository;
            _writtenOffMaterialRepository = writtenOffMaterialRepository;
            _userManager = userManager;
        }


        public List<DashboardTechniciansViewModel> GetTechnicianStats(DateTime? from, DateTime? to)
        {
            DateTime fromDate = from ?? DateTime.Now.AddMonths(-1);
            DateTime toDate = to ?? DateTime.Now;

            var writtenMaterials = _dashboardRepository.GetWrittenOffMaterialsBetween(fromDate, toDate)
                .GroupBy(w => w.ApplicationUser)
                .Select(group => new DashboardTechniciansViewModel
                {
                    TechnicianName = $"{group.Key!.FirstName} {group.Key.LastName}",
                    TechnicianId = group.Key.Id,
                    WrittenMaterialCount = group.Sum(w => w.Quantity),
                    DateFrom = fromDate,
                    DateTo = toDate
                }).OrderByDescending(x => x.WrittenMaterialCount).ToList();

            return writtenMaterials;
        }


        public List<DashboardMaterialsViewModel> GetMaterialStats(DateTime? from, DateTime? to)
        {
            DateTime fromDate = from ?? DateTime.Now.AddMonths(-1);
            DateTime toDate = to ?? DateTime.Now;

            var rawData = _dashboardRepository.GetWrittenOffMaterialsBetween(fromDate, toDate).ToList();

            var groupedData = rawData
               .GroupBy(w => w.Material.Id)
               .Select(group =>
                    {
                        var material = group.First().Material;
                        var topTechnicianGroup = group
                           .GroupBy(w => w.ApplicationUser)
                           .OrderByDescending(g => g.Sum(w => w.Quantity))
                           .FirstOrDefault();

                        return new DashboardMaterialsViewModel
                        {
                            MaterialId = material.Id,
                            MaterialName = material.Name,
                            WrittenMaterialCount = group.Sum(w => w.Quantity),
                            TopTechnicianName = topTechnicianGroup != null ? $"{topTechnicianGroup.Key!.FirstName} {topTechnicianGroup.Key.LastName}" : "N/A",
                            TopTechnicianQuantity = topTechnicianGroup?.Sum(w => w.Quantity) ?? 0,
                            MaterialType = material.TypeOfMaterial.Type,
                            DateFrom = fromDate,
                            DateTo = toDate
                        };
                    })
                    .OrderByDescending(x => x.WrittenMaterialCount)
                    .ToList();

            return groupedData;
        }


        public TechnicianStatsViewModel GetExtendedTechnicianStats(string technicianId, DateTime? from, DateTime? to)
        {
            DateTime fromDate = from ?? DateTime.Now.AddMonths(-1);
            DateTime toDate = to ?? DateTime.Now;

            var rawData = _dashboardRepository.GetWrittenOffMaterialsBetween(fromDate, toDate)
                .Where(x => x.ApplicationUserId == technicianId)
                .ToList();

            if (!rawData.Any())
            {
                throw new EntityNotFoundException("Technik nemá žádná data za zvolené období.");
            }
            var stats = rawData.GroupBy(w => w.Material)
                .Select(g => new TechnicianStatsViewModel.MaterialStat
                {
                    MaterialName = g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .ToList();

            var technician = rawData.FirstOrDefault()?.ApplicationUser;

            if (technician == null)
            {
                throw new EntityNotFoundException("Technik nebyl nalezen.");
            }
            return new TechnicianStatsViewModel
            {
                TechnicianName = $"{technician.FirstName} {technician.LastName}",
                DateFrom = fromDate,
                DateTo = toDate,
                MaterialStats = stats
            };
        }


        public async Task<MaterialStatsViewModel> GetExtendedMaterialStats(int materialId, DateTime? from, DateTime? to)
        {
            DateTime fromDate = from ?? DateTime.Now.AddMonths(-1);
            DateTime toDate = to ?? DateTime.Now;

            var material = await _materialRepository.GetWithTypeAsync(materialId);
            if (material == null)
            {
                throw new EntityNotFoundException("Materiál nebyl nalezen.");
            }

            var globalData = await _writtenOffMaterialRepository.GetAllWithDetailsAsync();
            var filteredGlobalData = globalData.Where(x => x.MaterialId == materialId).ToList();

            if (!filteredGlobalData.Any())
            {
                throw new EntityNotFoundException("Pro tento materiál nejsou dostupná žádná data.");
            }

            var quantityOfWrittenPieces = (ushort)_dashboardRepository
                .GetWrittenOffMaterialsBetween(fromDate, toDate)
                .Where(x => x.MaterialId == materialId)
                .Sum(x => x.Quantity);

            var stockInPieces = (ushort)(await _materialRepository.GetQuantityByMaterialIdAsync(materialId));

            var lastWeek = (ushort)filteredGlobalData
                .Where(x => x.WrittenOffDate >= DateTime.Now.AddDays(-7))
                .Sum(x => x.Quantity);

            var lastMonth = (ushort)filteredGlobalData
                .Where(x => x.WrittenOffDate >= DateTime.Now.AddMonths(-1))
                .Sum(x => x.Quantity);

            var lastQuarter = (ushort)filteredGlobalData
                .Where(x => x.WrittenOffDate >= DateTime.Now.AddMonths(-3))
                .Sum(x => x.Quantity);

            var lastYear = (ushort)filteredGlobalData
                .Where(x => x.WrittenOffDate >= DateTime.Now.AddYears(-1))
                .Sum(x => x.Quantity);

            return new MaterialStatsViewModel
            {
                DateFrom = fromDate,
                DateTo = toDate,
                MaterialName = material.Name,
                TypeOfMaterialName = material.TypeOfMaterial.Type ?? "Neznámý typ",
                QuantityOfWrittenPieces = quantityOfWrittenPieces,
                StockInPieces = stockInPieces,
                LastWeekConsumption = lastWeek,
                LastMonthConsumption = lastMonth,
                LastQuaterOfYearConsumption = lastQuarter,
                LastYearConsumption = lastYear,
                IsStateCritical = stockInPieces < material.CriticalQuantity ? true : false
            };
        }

        public async Task<List<CriticalMaterialViewModel>> GetCriticalMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllAsync();

            return materials
                .Where(m => m.Quantity <= m.CriticalQuantity)
                .Select(m => new CriticalMaterialViewModel
                {
                    MaterialName = m.Name,
                    CriticalQuantity = m.CriticalQuantity,
                    Quantity = m.Quantity
                }).ToList();
        }

        public List<MaterialChartItem> GetMaterialWrittenOffChart(string period)
        {
            DateTime fromDate = period.ToLower() switch
            {
                "week" => DateTime.Now.AddDays(-7),
                "month" => DateTime.Now.AddMonths(-1),
                "quarter" => DateTime.Now.AddMonths(-3),
                "year" => DateTime.Now.AddYears(-1),
                _ => DateTime.Now.AddMonths(-1)
            };

            return _dashboardRepository
                .GetWrittenOffMaterialsBetween(fromDate, DateTime.Now)
                .GroupBy(m => m.Material.Name)
                .Select(g => new MaterialChartItem
                {
                    MaterialName = g.Key,
                    WrittenOffCount = g.Sum(x => x.Quantity)
                })
                .OrderBy(x => x.MaterialName)
                .ToList();
        }

        public List<TechnicianChartItem> GetTechnicianWrittenOffChart(string period)
        {
            DateTime fromDate = period.ToLower() switch
            {
                "week" => DateTime.Now.AddDays(-7),
                "month" => DateTime.Now.AddMonths(-1),
                "quarter" => DateTime.Now.AddMonths(-3),
                "year" => DateTime.Now.AddYears(-1),
                _ => DateTime.Now.AddMonths(-1)
            };

            var rawData = _dashboardRepository
                .GetWrittenOffMaterialsBetween(fromDate, DateTime.Now)
                .Where(x => x.ApplicationUserId != null)
                .GroupBy(x => x.ApplicationUserId!)
                .Select(g => new
                {
                    TechnicianId = g.Key,
                    WrittenOffCount = g.Sum(x => x.Quantity)
                })
                .ToList();

            var technicianIds = rawData.Select(x => x.TechnicianId).ToList();

            var technicianNames = _userManager.Users
                .Where(u => technicianIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FullName })
                .ToDictionary(u => u.Id, u => u.FullName);

            return rawData
                .Select(x => new TechnicianChartItem
                {
                    TechnicianName = technicianNames.ContainsKey(x.TechnicianId) ? technicianNames[x.TechnicianId] : "Neznámý",
                    WrittenOffCount = x.WrittenOffCount
                })
                .OrderBy(x => x.TechnicianName)
                .ToList();
        }
    }
}