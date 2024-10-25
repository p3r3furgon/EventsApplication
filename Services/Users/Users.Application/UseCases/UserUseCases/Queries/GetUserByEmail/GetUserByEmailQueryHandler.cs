using MediatR;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserByEmailQueryHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<GetUserByEmailResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            GetUserByEmailQueryValidator validator = new();
            var results = await validator.ValidateAsync(request);
            if (!results.IsValid)
                throw new Exception(results.Errors.ToString());
            var user = await _usersRepository.GetByEmail(request.Email);
            if (user == null)
                throw new UserNotFoundException("User was not found");
            return new GetUserByEmailResponse(user);
        }
    }
}

