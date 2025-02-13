using AutoMapper;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Mapper_s
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserRegisterDTO, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.PasswordHash = BCrypt.Net.BCrypt.HashPassword(src.Password + src.UserEmail);
            });

            CreateMap<UserEntity, UserDTO>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
