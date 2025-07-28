using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Runtime.InteropServices.JavaScript;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;

namespace WareHouseSTARNET.Repositories.Implementations
{
    public class WrittenOffMaterialRepository : GenericsRepository<WrittenOffMaterial>, IWrittenOffMaterialRepository
    {
        public WrittenOffMaterialRepository(ApplicationDbContext context) : base(context)
        {
        
        }
        public async Task<IEnumerable<WrittenOffMaterial>> GetAllWithDetailsAsync(DateTime? date = null)
        {
            var query = dbSet
                .Include(x => x.Material)
                .ThenInclude(x => x.TypeOfMaterial)
                .Include(x => x.ApplicationUser)
                .AsQueryable();

            if (date.HasValue)
            {
                DateTime start = date.Value.Date;
                DateTime end = start.AddDays(1);
                query = query.Where(x => x.WrittenOffDate >= start && x.WrittenOffDate < end);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetMonthlyWrittenOffAsync(int materialId, string userId)
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            return await dbSet
                .Where(x => x.WrittenOffDate >= firstDayOfMonth && x.WrittenOffDate < firstDayOfNextMonth && x.MaterialId == materialId && x.ApplicationUserId == userId)
                .SumAsync(x => x.Quantity);
        }
    }
}
