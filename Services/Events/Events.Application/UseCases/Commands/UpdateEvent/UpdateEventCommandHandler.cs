using AutoMapper;
using CommonFiles.Interfaces;
using CommonFiles.Messaging;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using MassTransit;
using MediatR;
using Event = Events.Domain.Models.Event;

namespace Events.Application.UseCases.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEventCommandHandler(IEventsRepository eventsRepository, IFileService fileService, 
            IMapper mapper, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        {
            _eventsRepository = eventsRepository;
            _fileService = fileService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateEventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            UpdateEventCommandValidator validator = new();
            var results = await validator.ValidateAsync(request);

            if (!results.IsValid)
                throw new BadRequestException(results.Errors);

            var existingEvent = await _eventsRepository.GetById(request.Id);
            if (existingEvent == null)
                throw new EventNotFoundException(request.Id);
            if (existingEvent.Participants.Count > request.MaxParticipantNumber)
                throw new EventCapacityException(existingEvent.Participants.Count, request.MaxParticipantNumber); 

            string? oldImageName = existingEvent.Image;

            string? createdImageName = await _fileService.SaveFileAsync(request.Image, [".jpg", ".jpeg", ".png"]);
            if (!string.IsNullOrEmpty(createdImageName))
            {
                existingEvent.Image = createdImageName;
                _fileService.DeleteFile(oldImageName);
            }

            existingEvent = _mapper.Map(request, existingEvent);

            _eventsRepository.Update(existingEvent);
            await _unitOfWork.Save(cancellationToken);

            if (!string.IsNullOrEmpty(request.MessageTitle) &&
                    !string.IsNullOrEmpty(request.MessageContent))
                await SendMessages(request, existingEvent);

            return _mapper.Map<UpdateEventResponse>(existingEvent);
        }

        private async Task SendMessages(UpdateEventCommand request, Event @event)
        {
            List<Guid> usersId = @event.Participants.Select(p => p.UserId).ToList();
            var eventUpdated = _mapper.Map<EventUpdated>(request);
            eventUpdated.ParticipantsId = usersId;
            await _publishEndpoint.Publish(eventUpdated);
        }
    }

}
