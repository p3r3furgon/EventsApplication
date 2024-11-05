using AutoMapper;
using Users.Application.Dtos;
using Users.Domain.Models;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUsers
{
    public class GetUsersQueryMapper : Profile
    {
        public GetUsersQueryMapper()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
