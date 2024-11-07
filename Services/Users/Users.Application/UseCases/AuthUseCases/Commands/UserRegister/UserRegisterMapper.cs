using AutoMapper;
using Users.Application.Dtos;
using Users.Domain.Models;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserRegister
{
    public class UserRegisterMapper : Profile
    {
        public UserRegisterMapper()
        {
            CreateMap<UserRegisterCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => "User"))
                .IncludeMembers(src => src.UserDto);

            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.Parse(src.BirthDate)));
        }
    }
}
