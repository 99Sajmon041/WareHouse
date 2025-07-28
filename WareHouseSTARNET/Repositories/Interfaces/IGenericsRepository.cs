using System.Linq.Expressions;

namespace WareHouseSTARNET.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveAsync();
        Task<IEnumerable<TEntity>> FilterByOptionalValue(Expression<Func<TEntity, bool>> filter); 
        // v service udělat GetUserByID například..a použít tuto metodu formou (x => x.user.Id == id), tady jendou napíši, tam x-krát použiji :-)
    }
}