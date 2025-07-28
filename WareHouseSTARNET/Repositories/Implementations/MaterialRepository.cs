using Microsoft.EntityFrameworkCore;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;

namespace WareHouseSTARNET.Repositories.Implementations
{
    public class MaterialRepository : GenericsRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(ApplicationDbContext context) : base(context)
        {
          
        }

        public async Task<IEnumerable<Material>> GetAllWithTypeAsync()
        {
            return await dbSet.Include(x => x.TypeOfMaterial).ToListAsync();
        }

        public async Task<Material?> GetByNameAsync(string name)
        {
            var material = await dbSet.FirstOrDefaultAsync(x => x.Name == name);
            return material;
        }

        public async Task<Material?> GetWithTypeAsync(int id)
        {
            var material = await dbSet.Include(x => x.TypeOfMaterial).FirstOrDefaultAsync(x => x.Id == id);
            return material;
        }

        public async Task<int> GetQuantityByMaterialIdAsync(int id)
        {
            return await dbSet
                .Where(x => x.Id == id)
                .Select(x => x.Quantity)
                .FirstOrDefaultAsync();
        }
    }
}
