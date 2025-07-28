using Microsoft.EntityFrameworkCore;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;

namespace WareHouseSTARNET.Repositories.Implementations
{
    public class DashboardRepository : GenericsRepository<WrittenOffMaterial>, IDashboardRepository
    {
        public DashboardRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<WrittenOffMaterial> GetWrittenOffMaterialsBetween(DateTime from, DateTime to)
        {
            return dbSet
                .AsNoTracking()
                .Include(m => m.Material)
                .ThenInclude(m => m.TypeOfMaterial)
                .Include(m => m.ApplicationUser)          
                .Where(m => m.WrittenOffDate >= from && m.WrittenOffDate <= to);
        }
    }
}
