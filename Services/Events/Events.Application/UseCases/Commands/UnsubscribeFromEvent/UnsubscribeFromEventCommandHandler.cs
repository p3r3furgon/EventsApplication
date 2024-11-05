using CommonFiles.Interfaces;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Commands.UnsubscribeFromEvent
{
    public class UnsubscribeFromEventCommandHandler : IRequestHandler<UnsubscribeFromEventCommand, UnsubscribeFromEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParticipantsRepository _participantsRepository;

        public UnsubscribeFromEventCommandHandler(IEventsRepository eventsRepository, IUnitOfWork unitOfWork,
            IParticipantsRepository participantsRepository)
        {
            _eventsRepository = eventsRepository;
            _unitOfWork = unitOfWork;
            _participantsRepository = participantsRepository;
        }

        public async Task<UnsubscribeFromEventResponse> Handle(UnsubscribeFromEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId);
            if (@event == null)
                throw new EventNotFoundException(request.EventId);
            if (@event.Participants.Where(p => p.UserId == request.UserId).FirstOrDefault() == null)
                throw new BadRequestException("User is not subscribed to the event.");

            @event.Participants.RemoveAll(p => p.UserId == request.UserId);
            _eventsRepository.Update(@event);
            await _participantsRepository.DeleteByUserId(request.UserId);

            await _unitOfWork.Save(cancellationToken);
            return new UnsubscribeFromEventResponse("Success");
        }
    }

}
