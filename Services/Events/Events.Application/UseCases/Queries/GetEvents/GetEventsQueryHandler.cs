using AutoMapper;
using CommonFiles.Pagination;
using Events.Application.Dtos;
using Events.Domain.Interfaces.Repositories;
using Gridify;
using MediatR;
using System.Web;

namespace Events.Application.UseCases.Queries.GetEvents
{
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, GetEventsResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMapper _mapper;
        public GetEventsQueryHandler(IEventsRepository eventsRepository, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }
        public async Task<GetEventsResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _eventsRepository.Get(request.PaginationParams.PageNumber, request.PaginationParams.PageSize, 
                request.Filter);

            var eventsDto = _mapper.Map<List<EventResponseDto>>(events);

            var pagedResponse = new PagedResponse<EventResponseDto>(eventsDto, request.PaginationParams.PageNumber, 
                request.PaginationParams.PageSize);

            return new GetEventsResponse(pagedResponse);
        }
    }
}
