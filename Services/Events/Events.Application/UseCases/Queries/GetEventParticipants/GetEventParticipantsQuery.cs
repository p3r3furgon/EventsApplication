using MediatR;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public record GetEventParticipantsQuery(Guid Id): IRequest<List<GetEventParticipantsQueryResponse>>;

    public record GetEventParticipantsQueryResponse(string FirstName, string Surname, string Email, DateTime? RegistrationDateTime);
}
