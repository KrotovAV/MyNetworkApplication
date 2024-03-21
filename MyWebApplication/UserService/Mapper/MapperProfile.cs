using AutoMapper;
using UserService.BD;
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
/*
 * public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public RoleId RoleId { get; set; }
        public virtual Role Role { get; set; }

public int Id { get; set; }
        public string Username { get; set; }
        public RoleId RoleId { get; set; }
 * */