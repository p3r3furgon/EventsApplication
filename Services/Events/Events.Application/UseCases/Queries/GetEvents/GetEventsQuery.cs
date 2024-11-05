using CommonFiles.Pagination;
using Events.Application.Dtos;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEvents
{
    public record GetEventsQuery(PaginationParams PaginationParams, string? Filter)
        : IRequest<GetEventsResponse>;

    public record GetEventsResponse(PagedResponse<EventResponseDto> Events);
}
