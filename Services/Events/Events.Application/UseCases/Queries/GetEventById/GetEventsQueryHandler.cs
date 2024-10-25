using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdQueryResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        public GetEventByIdQueryHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;;
        }
        public async Task<GetEventByIdQueryResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            Event @event = await _eventsRepository.GetById(request.Id);
            if (@event == null)
                throw new EventNotFoundException(request.Id);

            var response = new GetEventByIdQueryResponse(@event);
            return response;
        }
    }
}
