using AutoMapper;
using MagicalVilla_Web.Models.Dto;

namespace MagicalVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();
			CreateMap<UserDto, RegistrationRequestDto>().ReverseMap();
		}
	}
}
