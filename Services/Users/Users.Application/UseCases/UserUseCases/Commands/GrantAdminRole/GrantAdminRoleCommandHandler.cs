using CommonFiles.Interfaces;
using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole
{
    public class GrantAdminRoleCommandHandler : IRequestHandler<GrantAdminRoleCommand, GrantAdminRoleResponse>
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GrantAdminRoleCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GrantAdminRoleResponse> Handle(GrantAdminRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException(request.Id);
            if (user.Role == "Admin")
                throw new BadRequestException("User is already an admin");
            user.Role = "Admin";
            _userRepository.Update(user);
            await _unitOfWork.Save(cancellationToken);
            return new GrantAdminRoleResponse(request.Id);
        }
    }
}
