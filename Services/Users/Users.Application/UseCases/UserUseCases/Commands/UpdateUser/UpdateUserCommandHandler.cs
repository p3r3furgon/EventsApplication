using AutoMapper;
using CommonFiles.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            user = _mapper.Map(request, user);
            user.PasswordHash = _passwordHasher.Generate(request.UserDto.Password);

            _userRepository.Update(user);
            await _unitOfWork.Save(cancellationToken);
            return new UpdateUserResponse(request.Id);
        }
    }
}
