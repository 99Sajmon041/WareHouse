using AutoMapper;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.ViewModels.MaterialViewModels;
using WareHouseSTARNET.ViewModels.TypeOfMaterialViewModels;
using WareHouseSTARNET.ViewModels.UserViewModels;
using WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels;

namespace WareHouseSTARNET
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WrittenOffMaterialCreateViewModel, WrittenOffMaterial>()
                .ForMember(dest => dest.Material, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore());

            CreateMap<WrittenOffMaterialUpdateViewModel, WrittenOffMaterial>()
                .ForMember(dest => dest.Material, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ApplicationUserCreateViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<TypeOfMaterial, TypeOfMaterialViewModel>().ReverseMap();
            CreateMap<TypeOfMaterialViewModel, TypeOfMaterialUpdateViewModel>();
            CreateMap<TypeOfMaterialCreateViewModel, TypeOfMaterial>();
            CreateMap<TypeOfMaterialUpdateViewModel, TypeOfMaterial>();

            CreateMap<MaterialCreateViewModel, Material>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
