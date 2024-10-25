using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole
{
    public class GrantAdminRoleCommandHandler : IRequestHandler<GrantAdminRoleCommand, GrantAdminRoleResponse>
    {
        private readonly IUsersRepository _userRepository;

        public GrantAdminRoleCommandHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }

        public async Task<GrantAdminRoleResponse> Handle(GrantAdminRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException(request.Id);
            if (user.Role == "Admin")
                throw new RoleAssignmentException("User is already an admin");
            user.Role = "Admin";
            await _userRepository.Update(user);
            return new GrantAdminRoleResponse(request.Id);
        }
    }
}
