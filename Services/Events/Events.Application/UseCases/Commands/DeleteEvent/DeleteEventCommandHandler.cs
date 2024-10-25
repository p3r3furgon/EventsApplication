using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, DeleteEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;

        public DeleteEventCommandHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<DeleteEventResponse> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {


            var @event = await _eventsRepository.GetById(request.Id);
            if (@event == null)
                throw new EventNotFoundException(request.Id);

            await _eventsRepository.Delete(request.Id);
            var response = new DeleteEventResponse(request.Id);
            return response;
        }
    }

}
