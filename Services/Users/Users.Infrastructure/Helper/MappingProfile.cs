using AutoMapper;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;
using Users.Persistance.Entities;

namespace Users.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenEntity>();
            CreateMap<RefreshTokenEntity, RefreshToken>();
        }
    }
}
