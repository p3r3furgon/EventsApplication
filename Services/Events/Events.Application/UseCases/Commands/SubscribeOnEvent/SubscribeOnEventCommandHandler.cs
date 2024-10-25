using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Commands.SubscribeOnEvent
{
    public class SubscribeOnEventCommandHandler : IRequestHandler<SubscribeOnEventCommand, SubscribeOnEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;

        public SubscribeOnEventCommandHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<SubscribeOnEventResponse> Handle(SubscribeOnEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId);

            if (@event == null)
                throw new EventNotFoundException(request.EventId);
            if (@event.Participants.Where(p => p.UserId == request.UserId).FirstOrDefault() != null)
                throw new EventParticipationException("User is already subscribed to the event.");
            if (@event.Participants.Count >= @event.MaxParticipantNumber)
                throw new EventParticipationException("There are no free spots in this event");  

            var participant = Participant.Create(request.UserId, request.FirstName, request.Surname, request.Email, DateTime.UtcNow);
            await _eventsRepository.AddParticipant(@event, participant);
            return new SubscribeOnEventResponse("Success");
        }
    }

}
