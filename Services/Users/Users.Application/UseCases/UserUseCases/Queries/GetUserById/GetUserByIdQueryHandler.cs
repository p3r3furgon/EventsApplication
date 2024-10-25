using MediatR;
using Users.Application.Exceptions;
using Users.Application.UseCases.UserUseCases.Queries.GetUserById;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUser
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserByIdQueryHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetById(request.Id);
            if (user == null)
                throw new UserNotFoundException(request.Id);
            return new GetUserByIdResponse(user);
        }
    }
}

