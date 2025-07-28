using AutoMapper;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.MaterialViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IFormHelperService _formHelperService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public MaterialService(IMaterialRepository repository,
            IMapper mapper,
            IFormHelperService formHelperService,
            IImageService imageService)
        {
            _materialRepository = repository;
            _mapper = mapper;
            _formHelperService = formHelperService;
            _imageService = imageService;
        }


        public async Task DeleteMaterialAsync(int id)
        {
            var existing = await _materialRepository.GetByIdAsync(id);
            if(existing == null)
            {
                throw new EntityNotFoundException($"Materiál s ID {id} nebyl nalezen");
            }
            await _materialRepository.DeleteAsync(existing);
        }


        public async Task<MaterialViewModel?> GetMaterialByIdAsync(int id)
        {
            var existing = await _materialRepository.GetWithTypeAsync(id);
            if (existing == null)
            {
                throw new EntityNotFoundException($"Materiál s ID {id} nebyl nalezen");
            }
            var material = new MaterialViewModel()
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                Quantity = existing.Quantity,
                CriticalQuantity = existing.CriticalQuantity,
                UnitPrice = existing.UnitPrice,
                ImageUrl = existing.ImageUrl,
                TypeOfMaterialName = existing.TypeOfMaterial.Type,
                TypeOfMaterialId = existing.TypeOfMaterialId
            };
            return material;
        }


        public async Task<IEnumerable<MaterialViewModel>> GetAllMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllWithTypeAsync();
            if (!materials.Any())
            {
                throw new EntityNotFoundException("Není zatím žádný materiál, je třeba ho prvně vytvořit!");
            }

            return materials.Select(m => new MaterialViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Quantity = m.Quantity,
                CriticalQuantity = m.CriticalQuantity,
                UnitPrice = m.UnitPrice,
                ImageUrl = m.ImageUrl,
                TypeOfMaterialName = m.TypeOfMaterial.Type,
                TypeOfMaterialId = m.TypeOfMaterialId
            });
        }


        public async Task UpdateMaterialAsync(MaterialUpdateViewModel updateModel)
        {
            var existing = await _materialRepository.GetByIdAsync(updateModel.Id);
            if(existing == null)
            {
                throw new EntityNotFoundException($"Materiál s tímto ID: {updateModel.Id} nebyl nalezen!");
            }

            if (updateModel.ImageFile != null && updateModel.ImageFile.Length > 0)
            {
                if(!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var fullOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.ImageUrl.TrimStart('/'));
                    if(File.Exists(fullOldPath))
                    {
                        File.Delete(fullOldPath);
                    }
                }
                var imageUrl = await _imageService.SaveImageAsync(updateModel.ImageFile);
                updateModel.ImageUrl = imageUrl;
            }

            existing.Name = updateModel.Name;
            existing.Description = updateModel.Description;
            existing.Quantity = updateModel.Quantity;
            existing.CriticalQuantity = updateModel.CriticalQuantity;
            existing.UnitPrice = updateModel.UnitPrice;
            existing.ImageUrl = updateModel.ImageUrl ?? existing.ImageUrl;
            existing.TypeOfMaterialId = updateModel.TypeOfMaterialId;

            await _materialRepository.SaveAsync();
        }


        public async Task CreateMaterialAsync(MaterialCreateViewModel createModel)
        {
            var existing = await _materialRepository.GetByNameAsync(createModel.Name);
            if (existing != null)
            {
                throw new EntityAlreadyExistsException($"Materiál: {createModel.Name} již existuje!");
            }

            string imageUrl = string.Empty;
            if (createModel.ImageFile != null && createModel.ImageFile.Length > 0)
            {
                imageUrl = await _imageService.SaveImageAsync(createModel.ImageFile);
            }

            var material = _mapper.Map<Material>(createModel);
            material.ImageUrl = imageUrl;

            await _materialRepository.CreateAsync(material);
        }

        public async Task AddQuantityAsync(int materialId, int quantityToAdd)
        {
            var existing = await _materialRepository.GetByIdAsync(materialId);
            if (existing != null && quantityToAdd > 0)
            {
                existing.Quantity += quantityToAdd;
                await _materialRepository.SaveAsync();
            }
        }


        // Method to get MaterialUpdateViewModel by ID for editing, instead of using MaterialViewModel and mapping it to ViewModel in the controller
        public async Task<MaterialUpdateViewModel> GetMaterialUpdateViewModelByIdAsync(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null)
            {
                throw new EntityNotFoundException($"Materiál s ID {id} nebyl nalezen.");
            }

            return new MaterialUpdateViewModel
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                Quantity = material.Quantity,
                CriticalQuantity = material.CriticalQuantity,
                UnitPrice = material.UnitPrice,
                ImageUrl = material.ImageUrl,
                TypeOfMaterialId = material.TypeOfMaterialId,
                TypeOfMaterials = await _formHelperService.GetTypeOfMaterialAsync()
            };
        }
    }
}
