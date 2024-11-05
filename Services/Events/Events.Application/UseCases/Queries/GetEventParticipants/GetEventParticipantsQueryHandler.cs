using AutoMapper;
using CommonFiles.Pagination;
using Events.Application.Dtos;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using MediatR;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public class GetEventParticipantsQueryHandler : IRequestHandler<GetEventParticipantsQuery, GetEventParticipantsResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IParticipantsRepository _participantsRepository;
        private readonly IMapper _mapper;
        public GetEventParticipantsQueryHandler(IEventsRepository eventsRepository, IMapper mapper,
            IParticipantsRepository participantsRepository)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
            _participantsRepository = participantsRepository;
        }
        public async Task<GetEventParticipantsResponse> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId);
            if (@event == null)
                throw new EventNotFoundException(request.EventId);

            var participants = _participantsRepository.GetByEventId(request.EventId,
                request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var participantsDto = _mapper.Map<List<ParticipantResponseDto>>(participants);

            var pagedResponse = new PagedResponse<ParticipantResponseDto>(participantsDto,
                request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            return new GetEventParticipantsResponse(pagedResponse);
        }
    }
}
