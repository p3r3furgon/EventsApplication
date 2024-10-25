using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using MediatR;

namespace Events.Application.UseCases.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(IEventsRepository eventsRepository, IFileService fileService, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            CreateEventCommandValidator validator = new();
            var results = await validator.ValidateAsync(request);

            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            string createdImageName = await _fileService.SaveFileAsync(request.Image, [".jpg", ".jpeg", ".png"]);
            var @event = _mapper.Map<Event>(request);
            @event.Image = createdImageName;

            await _eventsRepository.Create(@event);
            return _mapper.Map<CreateEventResponse>(@event);
        }
    }

}
