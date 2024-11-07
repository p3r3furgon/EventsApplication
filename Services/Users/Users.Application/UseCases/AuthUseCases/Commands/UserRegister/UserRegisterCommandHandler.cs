using AutoMapper;
using CommonFiles.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public UserRegisterCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserRegisterResponse> Handle(UserRegisterCommand request, 
            CancellationToken cancellationToken)
        {
            UserRegisterCommandValidator validator = new();
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            var user = await _usersRepository.GetByEmail(request.UserDto.Email);
            if (user != null)
                throw new BadRequestException("User with such email already exist");

            user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.Generate(request.UserDto.Password); ;

            await _usersRepository.Create(user);
            await _unitOfWork.Save(cancellationToken);

            return new UserRegisterResponse(user.Id);
        }
    }
}
