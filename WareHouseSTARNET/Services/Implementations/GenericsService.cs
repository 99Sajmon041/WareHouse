using AutoMapper;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;

namespace WareHouseSTARNET.Services.Implementations
{
    public class GenericsService<TCreateVM, TUpdateVM, TListVM, TEntity> : IGenericsService<TCreateVM, TUpdateVM, TListVM>
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IGenericRepository<TEntity> _repository;

        public GenericsService(IMapper mapper, IGenericRepository<TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<TListVM>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TListVM>>(entities);
        }

        public async Task<TUpdateVM?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? default : _mapper.Map<TUpdateVM>(entity);
        }

        public async Task CreateAsync(TCreateVM createModel)
        {
            var entity = _mapper.Map<TEntity>(createModel);
            await _repository.CreateAsync(entity);
        }

        public async Task UpdateAsync(TUpdateVM updateModel)
        {
            var entity = _mapper.Map<TEntity>(updateModel);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is not null)
            {
                await _repository.DeleteAsync(entity);
            }
        }
    }
}
