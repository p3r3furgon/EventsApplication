using MediatR;
using Users.Domain.Models;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id): IRequest<GetUserByIdResponse>;
    public record GetUserByIdResponse(User User);
}
