using AutoMapper;
using CommonFiles.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventCommandHandler(IEventsRepository eventsRepository, IFileService fileService,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _eventsRepository = eventsRepository;
            _fileService = fileService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            CreateEventCommandValidator validator = new();
            var results = await validator.ValidateAsync(request);

            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            string createdImageName = await _fileService.SaveFileAsync(request.EventDto.Image, [".jpg", ".jpeg", ".png"]);
            var @event = _mapper.Map<Event>(request);
            @event.Image = createdImageName;

            await _eventsRepository.Create(@event);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CreateEventResponse>(@event);
        }
    }

}
