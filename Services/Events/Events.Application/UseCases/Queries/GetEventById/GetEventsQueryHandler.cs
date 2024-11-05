using AutoMapper;
using Events.Application.Dtos;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMapper _mapper;
        public GetEventByIdQueryHandler(IEventsRepository eventsRepository, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }
        public async Task<GetEventByIdResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Id);
            if (@event == null)
                throw new EventNotFoundException(request.Id);

            var eventDto = _mapper.Map<EventResponseDto>(@event);

            var response = new GetEventByIdResponse(eventDto);
            return response;
        }
    }
}
