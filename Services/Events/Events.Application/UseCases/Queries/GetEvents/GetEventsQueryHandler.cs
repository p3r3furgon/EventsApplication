using AutoMapper;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEvents
{
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<GetEventsQueryResponse>>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMapper _mapper;
        public GetEventsQueryHandler(IEventsRepository eventsRepository, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }
        public async Task<List<GetEventsQueryResponse>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _eventsRepository.Get();
            return _mapper.Map<List<GetEventsQueryResponse>>(events);
        }
    }
}
