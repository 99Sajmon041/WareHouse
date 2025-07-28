using AutoMapper;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class TypeOfMaterialService : ITypeOfMaterialService
    {
        private readonly IMapper _mapper;
        private readonly ITypeOfMaterialRepository _typeOfMaterialRepository;

        public TypeOfMaterialService(IMapper mapper, ITypeOfMaterialRepository typeOfMaterialRepository)
        {
            _mapper = mapper;
            _typeOfMaterialRepository = typeOfMaterialRepository;
        }

        public async Task DeleteTypeOfMaterialAsync(int id)
        {
            var existing = await _typeOfMaterialRepository.GetByIdAsync(id);
            if(existing == null)
            {
                throw new EntityNotFoundException($"Typ materiálus ID: {id} nebyl nalezen!");
            }
            await _typeOfMaterialRepository.DeleteAsync(existing);
        }

        public async Task<IEnumerable<TypeOfMaterialViewModel>> GetAllTypeOfMaterialsAsync()
        {
            var TypesOfmaterial = await _typeOfMaterialRepository.GetAllAsync();
            var types = _mapper.Map<List<TypeOfMaterialViewModel>>(TypesOfmaterial);
            return types;
        }

        public async Task<TypeOfMaterialViewModel> GetTypeOfMaterialByIdAsync(int id)
        {
            var existing = await _typeOfMaterialRepository.GetByIdAsync(id);
            if(existing == null)
            {
                throw new EntityNotFoundException($"Typ mateirálu s tímto ID: {id} nebyl nalezen!");
            }
            var typeOfMaterial = _mapper.Map<TypeOfMaterialViewModel>(existing);
            return typeOfMaterial;
        }

        public async Task UpdateTypeOfMaterialAsync(TypeOfMaterialUpdateViewModel updateModel)
        {
            var existing = await _typeOfMaterialRepository.GetByIdAsync(updateModel.Id);
            if (existing == null)
            {
                throw new EntityNotFoundException($"Typ mateirálu s tímto ID: {updateModel.Id} nebyl nalezen!");
            }
            _mapper.Map(updateModel, existing);
            await _typeOfMaterialRepository.UpdateAsync(existing);
        }

        public async Task CreateTypeOfMaterialAsync(TypeOfMaterialCreateViewModel createModel)
        {
            var existing = await _typeOfMaterialRepository.GetTypeByNameAsync(createModel.Type);
            if (existing != null)
            {
                throw new EntityAlreadyExistsException($"Tento typ materiálu: {createModel.Type} již existuje!");
            }
            var model = _mapper.Map<TypeOfMaterial>(createModel);
            await _typeOfMaterialRepository.CreateAsync(model);
        }
    }
}
