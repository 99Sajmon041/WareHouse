using Microsoft.EntityFrameworkCore;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;

namespace WareHouseSTARNET.Repositories.Implementations
{
    public class TypeOfMaterialRepository : GenericsRepository<TypeOfMaterial>, ITypeOfMaterialRepository
    {
        public TypeOfMaterialRepository(ApplicationDbContext context) : base (context)
        {
            
        }
        public async Task<TypeOfMaterial?> GetTypeByIdAsync(int id)
        {
            var typeOfMaterial = await dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return typeOfMaterial;
        }

        public async Task<TypeOfMaterial?> GetTypeByNameAsync(string type)
        {
            var typeOfMaterial = await dbSet.FirstOrDefaultAsync(x => x.Type == type);
            return typeOfMaterial;
        }
    }
}
