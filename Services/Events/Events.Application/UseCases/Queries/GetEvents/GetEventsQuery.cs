using MediatR;

namespace Events.Application.UseCases.Queries.GetEvents
{
    public record GetEventsQuery(): IRequest<List<GetEventsQueryResponse>>;

    public record GetEventsQueryResponse(Guid Id, string Title, string? Description, DateTime? DateTime, 
        int MaxParticipantNumber, int? ParticipantsNumber, string? Image);
}
