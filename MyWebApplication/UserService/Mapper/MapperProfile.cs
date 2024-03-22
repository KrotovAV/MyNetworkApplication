using AutoMapper;
using DataBaseUsers.BD;
using UserService.Dto;

namespace UserService.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDtoUs, User >()
                .ForMember(dect => dect.Id, opt => opt.MapFrom(y=>y.Id))
                .ForMember(dect => dect.Name, opt => opt.MapFrom(y => y.Username))
                .ForMember(dect => dect.Password, opt => opt.Ignore())
                .ForMember(dect => dect.Salt, opt => opt.Ignore())
                .ForMember(dect => dect.RoleId, opt => opt.MapFrom(y => y.RoleId))
                .ReverseMap();

        }
    }
}
