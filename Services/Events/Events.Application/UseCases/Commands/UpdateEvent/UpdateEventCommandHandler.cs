using AutoMapper;
using CommonFiles.Messaging;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using MassTransit;
using MediatR;

namespace Events.Application.UseCases.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventResponse>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateEventCommandHandler(IEventsRepository eventsRepository, IFileService fileService, 
            IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _eventsRepository = eventsRepository;
            _fileService = fileService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
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

            string? oldImage = existingEvent.Image;

            string createdImageName = await _fileService.SaveFileAsync(request.Image, [".jpg", ".jpeg", ".png"]);
            if (!string.IsNullOrEmpty(createdImageName))
                _fileService.DeleteFile(oldImage);

            existingEvent.Title = (string.IsNullOrEmpty(request.Title)) ? existingEvent.Title : request.Title;
            existingEvent.Description = (string.IsNullOrEmpty(request.Description)) ? existingEvent.Description : request.Description;
            existingEvent.DateTime = request.DateTime ?? existingEvent.DateTime;
            existingEvent.Category = (string.IsNullOrEmpty(request.Category)) ? existingEvent.Category : request.Category;
            existingEvent.Place = (string.IsNullOrEmpty(request.Place)) ? existingEvent.Place : request.Place;
            existingEvent.MaxParticipantNumber = request.MaxParticipantNumber ?? existingEvent.MaxParticipantNumber;
            existingEvent.Image = (string.IsNullOrEmpty(createdImageName)) ? existingEvent.Image : createdImageName;

            await _eventsRepository.Update(existingEvent);

            if (!string.IsNullOrEmpty(request.MessageTitle) &&
                    !string.IsNullOrEmpty(request.MessageDescription))
            {
                List<Guid> usersId = existingEvent.Participants.Select(p => p.UserId).ToList();
                var eventUpdated = _mapper.Map<EventUpdated>(request);
                eventUpdated.ParticipantsId = usersId;
                await _publishEndpoint.Publish(eventUpdated);
            }

            return _mapper.Map<UpdateEventResponse>(existingEvent);
        }
    }

}
