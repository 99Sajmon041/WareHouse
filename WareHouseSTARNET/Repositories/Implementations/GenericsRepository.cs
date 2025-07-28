using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Repositories.Interfaces;

namespace WareHouseSTARNET.Repositories.Implementations
{
    public class GenericsRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> dbSet;
        protected readonly ApplicationDbContext _context;

        public GenericsRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }
        public async Task<T> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> entities = await dbSet.ToListAsync();
            return entities;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FilterByOptionalValue(Expression<Func<T, bool>> filter)
        {
            var result = await dbSet.Where(filter).ToListAsync();
            return result;
        }
    }
}
