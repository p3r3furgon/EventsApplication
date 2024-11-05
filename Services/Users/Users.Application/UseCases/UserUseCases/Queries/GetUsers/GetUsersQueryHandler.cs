using AutoMapper;
using CommonFiles.Pagination;
using MediatR;
using Users.Application.Dtos;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models;

namespace Users.Application.UseCases.UserUseCases.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _usersRepository.Get(request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var usersDto = _mapper.Map<List<UserResponseDto>>(users);

            var pagedResponse = new PagedResponse<UserResponseDto>(usersDto, request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize);
            return new GetUsersResponse(pagedResponse);
        }
    }
}

