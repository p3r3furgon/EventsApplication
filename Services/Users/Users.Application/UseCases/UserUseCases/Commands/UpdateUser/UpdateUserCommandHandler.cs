using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUsersRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            UpdateUserCommandValidator validator = new();

            var results = await validator.ValidateAsync(request);

            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            var user = await _userRepository.GetById(request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            var passwordHash = string.IsNullOrEmpty(request.Password) ? "" : _passwordHasher.Generate(request.Password);
            DateOnly? birthDate = string.IsNullOrEmpty(request.BirthDate) ? null : DateOnly.Parse(request.BirthDate);

            user.FirstName = (string.IsNullOrEmpty(request.FirstName)) ? user.FirstName : request.FirstName;
            user.Surname = (string.IsNullOrEmpty(request.Surname)) ? user.Surname : request.Surname;
            user.BirthDate = birthDate ?? user.BirthDate;
            user.Email = (string.IsNullOrEmpty(request.Email)) ? user.Email : request.Email;
            user.PasswordHash = (string.IsNullOrEmpty(passwordHash)) ? user.PasswordHash : user.PasswordHash;

            await _userRepository.Update(user);

            return new UpdateUserResponse(request.Id);
        }
    }
}
