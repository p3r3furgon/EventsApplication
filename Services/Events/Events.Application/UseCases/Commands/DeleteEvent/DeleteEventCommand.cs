using MediatR;

namespace Events.Application.UseCases.Commands.DeleteEvent
{
    public record DeleteEventCommand(Guid Id) : IRequest<DeleteEventResponse>;
    public record DeleteEventResponse(Guid Id);
}
