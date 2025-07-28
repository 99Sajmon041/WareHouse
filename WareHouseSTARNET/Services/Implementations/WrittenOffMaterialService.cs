using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WareHouseSTARNET.Exceptions;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Common;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class WrittenOffMaterialService : IWrittenOffMaterialService
    {
        private readonly IWrittenOffMaterialRepository _writtenOfMaterialrepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public WrittenOffMaterialService(IWrittenOffMaterialRepository writtenOfMaterialrepository,
            IMapper mapper, IMaterialRepository materialRepository, UserManager<ApplicationUser> userManager)
        {
            _writtenOfMaterialrepository = writtenOfMaterialrepository;
            _materialRepository = materialRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task DeleteWrittenMaterialAsync(int id)
        {
            var existingMaterial = await _writtenOfMaterialrepository.GetByIdAsync(id);
            if(existingMaterial == null)
            {
                throw new EntityNotFoundException($"Materiál s ID: {id} nebyl nalezen!");
            }
            var material = await _materialRepository.GetByIdAsync(existingMaterial.MaterialId);
            if(material == null)
            {
                throw new EntityNotFoundException("Navázaný materiál již neexistuje, nelze přičíst zpět množství.");
            }
            material.Quantity += existingMaterial.Quantity;
            await _materialRepository.UpdateAsync(material);
            await _writtenOfMaterialrepository.DeleteAsync(existingMaterial);
        }

        public async Task<IEnumerable<WrittenOffMaterialViewModel>> GetAllWrittenMaterialsAsync()
        {
            var listOfMaterials = await _writtenOfMaterialrepository.GetAllWithDetailsAsync();
            var writtenMaterials = listOfMaterials.Select(x => new WrittenOffMaterialViewModel
            {
                Id = x.Id,
                WritteOfDate = x.WrittenOffDate,
                Quantity = x.Quantity,
                MaterialName = x.Material.Name,
                TypeOfMaterialName = x.Material.TypeOfMaterial.Type,
                ApplicationUser = x.ApplicationUser?.FullName ?? "Neznámý uživatel"
            });
            return writtenMaterials;
        }

        public async Task<IEnumerable<WrittenOffMaterialViewModel>> GetAllWithDetailsAsync(DateTime dateTime)
        {
            var materials = await _writtenOfMaterialrepository.GetAllWithDetailsAsync(dateTime);
            var writtenMaterials = materials.Select(x => new WrittenOffMaterialViewModel
            {
                Id = x.Id,
                WritteOfDate = x.WrittenOffDate,
                Quantity = x.Quantity,
                MaterialName = x.Material.Name,
                TypeOfMaterialName = x.Material.TypeOfMaterial.Type,
                ApplicationUser = x.ApplicationUser?.FullName ?? "Neznámý uživatel"
            });
            return writtenMaterials;
        }

        public async Task CreateWrittenMaterialAsync(WrittenOffMaterialCreateViewModel createModel)
        {
            var existing = await _materialRepository.GetByIdAsync(createModel.MaterialId);
            if (existing == null)
            {
                throw new EntityNotFoundException("Zvolený materiál neexistuje.");
            }
            var user = await _userManager.FindByIdAsync(createModel.ApplicationUserId);
            if (user == null)
            {
                throw new EntityNotFoundException("Zvolený uživatel neexistuje.");
            }
            if (createModel.Quantity > existing.Quantity)
            {
                throw new ForbiddenOperationException("Není možné odepsat materiál, množství je větší, než je množství skladem!");
            }
            var monthlyWrittenOff = await _writtenOfMaterialrepository.GetMonthlyWrittenOffAsync(createModel.MaterialId, user.Id);
            if ((createModel.Quantity + monthlyWrittenOff) > AppConstants.MaxMonthlyWrittenOff)
            {
                throw new ForbiddenOperationException($"Není možné odepsat materiál, překročil by se maximální měsíční limit uživatele: {AppConstants.MaxMonthlyWrittenOff}!");
            }

            existing.Quantity -= createModel.Quantity;
            await _materialRepository.UpdateAsync(existing);
            var material = _mapper.Map<WrittenOffMaterial>(createModel);
            await _writtenOfMaterialrepository.CreateAsync(material);
        }
    }
}
 