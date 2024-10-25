using AutoMapper;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public class GetEventParticipantsQueryHandler : IRequestHandler<GetEventParticipantsQuery, List<GetEventParticipantsQueryResponse>>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMapper _mapper;
        public GetEventParticipantsQueryHandler(IEventsRepository eventsRepository, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }
        public async Task<List<GetEventParticipantsQueryResponse>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Id);
            if (@event == null)
                throw new Exception("Event not found"); // todo: add notfound exc

            return _mapper.Map<List<GetEventParticipantsQueryResponse>>(@event.Participants);
        }
    }
}
