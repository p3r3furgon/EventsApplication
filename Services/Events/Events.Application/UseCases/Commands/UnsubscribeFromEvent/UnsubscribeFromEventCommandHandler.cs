using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Commands.UnsubscribeFromEvent
{
    public class UnsubscribeFromEventCommandHandler : IRequestHandler<UnsubscribeFromEventCommand, UnsubscribeFromEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;

        public UnsubscribeFromEventCommandHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<UnsubscribeFromEventResponse> Handle(UnsubscribeFromEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId);
            if (@event == null)
                throw new EventNotFoundException(request.EventId);
            if (@event.Participants.Where(p => p.UserId == request.UserId).FirstOrDefault() == null)
                throw new EventParticipationException("User is not subscribed to the event.");

            @event.Participants.RemoveAll(p => p.UserId == request.UserId);
            await _eventsRepository.RemoveParticipant(@event.Id, request.UserId);
            return new UnsubscribeFromEventResponse("Success");
        }
    }

}
