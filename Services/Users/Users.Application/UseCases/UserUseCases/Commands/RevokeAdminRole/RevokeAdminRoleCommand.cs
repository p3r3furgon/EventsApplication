using MediatR;

namespace Users.Application.UseCases.UserUseCases.Commands.RevokeAdminRole
{
    namespace Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole
    {
        public record RevokeAdminRoleCommand(Guid Id) : IRequest<RevokeAdminRoleResponse>;
        public record RevokeAdminRoleResponse(Guid Id);
    }
}
