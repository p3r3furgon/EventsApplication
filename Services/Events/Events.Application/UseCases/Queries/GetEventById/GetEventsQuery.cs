using Events.Application.Dtos;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventById
{
    public record GetEventByIdQuery(Guid Id): IRequest<GetEventByIdResponse>;

    public record GetEventByIdResponse(EventResponseDto Event);
}
