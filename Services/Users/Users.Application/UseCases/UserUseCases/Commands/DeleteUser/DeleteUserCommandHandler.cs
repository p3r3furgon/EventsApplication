using MediatR;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUsersRepository _userRepository;

        public DeleteUserCommandHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new Exception("User not found");
            await _userRepository.Delete(request.Id);
            return new DeleteUserResponse(request.Id);
        }
    }
}
