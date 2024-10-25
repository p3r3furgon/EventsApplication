using AutoMapper;
using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserRegister
{
    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, UserRegisterResponse>
    {
        
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserRegisterCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        public async Task<UserRegisterResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            UserRegisterCommandValidator validator = new();
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            var user = await _usersRepository.GetByEmail(request.Email);
            if (user != null)
                throw new BadRequestException("User with such email already exist");

            string passwordHash = _passwordHasher.Generate(request.Password);

            user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;

            await _usersRepository.Create(user);
            return new UserRegisterResponse(user.Id);
        }
    }
}
