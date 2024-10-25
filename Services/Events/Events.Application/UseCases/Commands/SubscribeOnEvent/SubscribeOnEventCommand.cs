using MediatR;

namespace Events.Application.UseCases.Commands.SubscribeOnEvent
{
    public record SubscribeOnEventCommand(Guid EventId, string FirstName, string Surname, 
        string Email, Guid UserId) : IRequest<SubscribeOnEventResponse>;
    public record SubscribeOnEventResponse(string Message);
}
