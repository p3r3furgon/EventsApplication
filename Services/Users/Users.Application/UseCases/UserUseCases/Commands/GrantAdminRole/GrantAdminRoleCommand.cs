using MediatR;

namespace Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole
{
    public record GrantAdminRoleCommand(Guid Id) : IRequest<GrantAdminRoleResponse>;
    public record GrantAdminRoleResponse(Guid Id);
}
