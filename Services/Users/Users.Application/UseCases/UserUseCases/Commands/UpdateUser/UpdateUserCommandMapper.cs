using AutoMapper;
using Users.Domain.Models;

namespace Users.Application.UseCases.UserUseCases.Commands.UpdateUser
{
    public class UpdateUserCommandMapper : Profile
    {
        public UpdateUserCommandMapper()
        {
            CreateMap<UpdateUserCommand, User>()
                .BeforeMap((src, dest) =>
                {
                    if (!string.IsNullOrEmpty(src.BirthDate))
                    {
                        dest.BirthDate = DateOnly.Parse(src.BirthDate);
                    }
                })
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.Condition(s => s.FirstName != null))
                .ForMember(dest => dest.Surname, opt => opt.Condition(s => s.Surname != null))
                .ForMember(dest => dest.Email, opt => opt.Condition(s => s.Email != null));
        }
    }
}
