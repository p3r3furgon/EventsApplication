using CommonFiles.Interfaces;
using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException("User not found");
            await _userRepository.Delete(request.Id);
            await _unitOfWork.Save(cancellationToken);
            return new DeleteUserResponse(request.Id);
        }
    }
}
