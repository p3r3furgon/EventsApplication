using CommonFiles.Messaging;
using Events.API.Dtos;
using Events.API.Validators;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Gridify;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Event = Events.Domain.Models.Event;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        private readonly IFileService _fileService;
        private readonly IPublishEndpoint _publishEndpoint;

        public EventsController(IEventsService eventsService, IFileService fileService, IPublishEndpoint publishEndpoint)
        {
            _eventsService = eventsService;
            _fileService = fileService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] GridifyQuery gridifyQuery)
        {
            var events = await _eventsService.GetEvents();
            var eventsDto = events
                .Select(e => new EventResponceDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    DateTime = e.DateTime,
                    MaxParticipantNumber = e.MaxParticipantNumber,
                    ParticipantsNumber = e.Participants.Count,
                    Image = e.Image
                }).ToList();

            var result = eventsDto.AsQueryable()
                          .ApplyFiltering(gridifyQuery)
                          .ApplyOrdering(gridifyQuery)
                          .ApplyPaging(gridifyQuery.Page, gridifyQuery.PageSize);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var @event = await _eventsService.GetEventById(id);
            return StatusCode(StatusCodes.Status200OK,
                @event);
        }

        [HttpGet("{id}/paricipants")]
        [Authorize]
        public async Task<IActionResult> GetEventParticipants(Guid id)
        {
            var @event = await _eventsService.GetEventById(id);
            var participantsDto = @event.Participants
                .Select(e => new ParticipantResponceDto
                {
                    FirstName = e.FirstName,
                    Surname = e.Surname,
                    Email = e.Email,
                    RegistrationDateTime = e.RegistrationDateTime
                }).ToList();

            return StatusCode(StatusCodes.Status200OK, participantsDto);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequest createEventRequest)
        {
            CreateEventRequestValidator validator = new ();
            var results = await validator.ValidateAsync(createEventRequest);

            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            string createdImageName = await _fileService.SaveFileAsync(createEventRequest.Image, [".jpg", ".jpeg", ".png"]);
            var @event = Event.Create(createEventRequest.Title, createEventRequest.Description,
                createEventRequest.DateTime, createEventRequest.Place, createEventRequest.Category, createEventRequest.MaxparticipantNumber,
                new List<Participant>(), createdImageName);
            var eventId = await _eventsService.CreateEvent(@event);
            return StatusCode(StatusCodes.Status201Created, eventId);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromForm] UpdateEventRequest updateEventRequest)
        {
            UpdateEventRequestValidator validator = new();
            var results = await validator.ValidateAsync(updateEventRequest);

            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            var existingEvent = await _eventsService.GetEventById(id);
            if(existingEvent.Participants.Count > updateEventRequest.MaxparticipantNumber)
                 return StatusCode(StatusCodes.Status400BadRequest, results.Errors);

            string? oldImage = existingEvent.Image;

            string createdImageName = await _fileService.SaveFileAsync(updateEventRequest.Image, [".jpg", ".jpeg", ".png"]);
            if(!string.IsNullOrEmpty(createdImageName))
                _fileService.DeleteFile(oldImage);

            var eventId = await _eventsService.UpdateEvent(id, updateEventRequest.Title, updateEventRequest.Description,
                updateEventRequest.DateTime, updateEventRequest.Category, updateEventRequest.Place, updateEventRequest.MaxparticipantNumber, createdImageName);

            if(!string.IsNullOrEmpty(updateEventRequest.MessageTitle) &&
                !string.IsNullOrEmpty(updateEventRequest.MessageDescription))
            {
                List<Guid> usersId = existingEvent.Participants.Select(p => p.UserId).ToList();
                EventUpdated eventUpdated = new()
                {
                    Title = updateEventRequest.MessageTitle,
                    Message = updateEventRequest.MessageDescription,
                    DateTime = DateTime.UtcNow,
                    ParticipantsId = usersId
                };
                await _publishEndpoint.Publish<EventUpdated>(eventUpdated);
            }

            return StatusCode(StatusCodes.Status200OK, eventId);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _eventsService.GetEventById(id);
            await _eventsService.DeleteEvent(id);

            if(!string.IsNullOrEmpty(@event.Image))
                _fileService.DeleteFile(@event.Image);

            return StatusCode(StatusCodes.Status200OK, id);
        }

        [HttpPost("subscribe")]
        [Authorize]
        public  async Task<IActionResult> RegisterUserOnEvent(Guid eventId)
        {
            var firstName = User?.FindFirstValue(ClaimTypes.Name);
            var surname = User?.FindFirstValue(ClaimTypes.Surname);
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            await _eventsService.RegisterUserOnEvent(eventId, firstName, surname, email, userId);
            return StatusCode(StatusCodes.Status200OK, "You have subscribed on event");
        }

        [HttpDelete("unsubscribe")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromEvent(Guid eventId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            await _eventsService.UnsubscribeFromEvent(eventId, Guid.Parse(userId));
            return StatusCode(StatusCodes.Status200OK, "You unsubscribed from event");
        }


    }
}
