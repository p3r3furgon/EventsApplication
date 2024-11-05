using CommonFiles.Interfaces;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, DeleteEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventCommandHandler(IEventsRepository eventsRepository, IUnitOfWork unitOfWork)
        {
            _eventsRepository = eventsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteEventResponse> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Id);
            if (@event == null)
                throw new EventNotFoundException(request.Id);

            await _eventsRepository.Delete(request.Id);
            await _unitOfWork.Save(cancellationToken);

            return new DeleteEventResponse(request.Id);
        }
    }

}
