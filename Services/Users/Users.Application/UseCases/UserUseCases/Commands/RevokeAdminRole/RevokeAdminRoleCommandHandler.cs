using CommonFiles.Interfaces;
using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.RevokeAdminRole
{
    public class RevokeAdminRoleCommandHandler : IRequestHandler<RevokeAdminRoleCommand, RevokeAdminRoleResponse>
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeAdminRoleCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RevokeAdminRoleResponse> Handle(RevokeAdminRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException(request.Id);
            if (user.Role == "User")
                throw new BadRequestException("User is not an admin.");
            user.Role = "User";
            _userRepository.Update(user);
            await _unitOfWork.Save(cancellationToken);
            return new RevokeAdminRoleResponse(request.Id);
        }
    }
}
