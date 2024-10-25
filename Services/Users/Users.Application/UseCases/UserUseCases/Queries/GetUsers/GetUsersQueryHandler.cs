using MediatR;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUsersQueryHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _usersRepository.Get();
            return new GetUsersResponse(users);
        }
    }
}

