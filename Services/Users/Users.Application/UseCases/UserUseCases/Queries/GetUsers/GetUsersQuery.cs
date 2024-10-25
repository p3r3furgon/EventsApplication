using MediatR;
using Users.Domain.Models;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUsers
{
    public record GetUsersQuery(): IRequest<GetUsersResponse>;
    public record GetUsersResponse(List<User> Users);
}
