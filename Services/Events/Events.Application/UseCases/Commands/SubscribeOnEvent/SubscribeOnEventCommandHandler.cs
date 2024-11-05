using CommonFiles.Interfaces;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Commands.SubscribeOnEvent
{
    public class SubscribeOnEventCommandHandler : IRequestHandler<SubscribeOnEventCommand, SubscribeOnEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParticipantsRepository _participantsRepository;

        public SubscribeOnEventCommandHandler(IEventsRepository eventsRepository, 
            IUnitOfWork unitOfWork, IParticipantsRepository participantsRepository)
        {
            _eventsRepository = eventsRepository;
            _participantsRepository = participantsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscribeOnEventResponse> Handle(SubscribeOnEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId);

            if (@event == null)
                throw new EventNotFoundException(request.EventId);
            if (@event.Participants.Where(p => p.UserId == request.UserId).FirstOrDefault() != null)
                throw new BadRequestException("User is already subscribed to the event.");
            if (@event.Participants.Count >= @event.MaxParticipantNumber)
                throw new BadRequestException("There are no free spots in this event");  

            var participant = Participant.Create(request.UserId, request.FirstName, request.Surname, request.Email, DateTime.UtcNow);

            await _participantsRepository.Add(participant);
            @event.Participants.Add(participant);
            _eventsRepository.Update(@event);

            await _unitOfWork.Save(cancellationToken);

            return new SubscribeOnEventResponse("Success");
        }
    }

}
