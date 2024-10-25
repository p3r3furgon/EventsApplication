using MediatR;
using Users.Application.Exceptions;
using Users.Application.UseCases.UserUseCases.Commands.RevokeAdminRole.Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.RevokeAdminRole
{
    public class RevokeAdminRoleCommandHandler : IRequestHandler<RevokeAdminRoleCommand, RevokeAdminRoleResponse>
    {
        private readonly IUsersRepository _userRepository;

        public RevokeAdminRoleCommandHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }

        public async Task<RevokeAdminRoleResponse> Handle(RevokeAdminRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException(request.Id);
            if (user.Role == "User")
                throw new RoleAssignmentException("User is not an admin.");
            user.Role = "User";
            await _userRepository.Update(user);
            return new RevokeAdminRoleResponse(request.Id);
        }
    }
}
