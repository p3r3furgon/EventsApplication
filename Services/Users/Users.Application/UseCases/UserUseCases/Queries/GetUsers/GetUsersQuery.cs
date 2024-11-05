using CommonFiles.Pagination;
using MediatR;
using Users.Application.Dtos;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUsers
{
    public record GetUsersQuery(PaginationParams PaginationParams): IRequest<GetUsersResponse>;
    public record GetUsersResponse(PagedResponse<UserResponseDto> Users);
}
