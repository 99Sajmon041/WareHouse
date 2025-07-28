namespace WareHouseSTARNET.Services.Interfaces
{
    public interface IGenericsService<TCreateVM, TUpdateVM, TListVM>
    {
        Task<IEnumerable<TListVM>> GetAllAsync();
        Task<TUpdateVM?> GetByIdAsync(int id);
        Task CreateAsync(TCreateVM createModel);
        Task UpdateAsync(TUpdateVM updateModel);
        Task DeleteAsync(int id);
    }
}
