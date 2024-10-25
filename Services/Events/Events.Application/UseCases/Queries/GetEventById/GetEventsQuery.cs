using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventById
{
    public record GetEventByIdQuery(Guid Id): IRequest<GetEventByIdQueryResponse>;

    public record GetEventByIdQueryResponse(Event Event);
}
