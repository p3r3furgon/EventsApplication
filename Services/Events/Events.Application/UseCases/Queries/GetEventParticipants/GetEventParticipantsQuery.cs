using CommonFiles.Pagination;
using Events.Application.Dtos;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public record GetEventParticipantsQuery(Guid EventId, PaginationParams PaginationParams): IRequest<GetEventParticipantsResponse>;

    public record GetEventParticipantsResponse(PagedResponse<ParticipantResponseDto> Participants);
}
