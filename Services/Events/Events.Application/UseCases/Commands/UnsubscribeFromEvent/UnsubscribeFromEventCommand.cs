using MediatR;

namespace Events.Application.UseCases.Commands.UnsubscribeFromEvent
{
    public record UnsubscribeFromEventCommand(Guid EventId, Guid UserId) : IRequest<UnsubscribeFromEventResponse>;
    public record UnsubscribeFromEventResponse(string Message);
}
