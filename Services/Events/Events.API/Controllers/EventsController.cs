using Events.API.Dtos;
using Events.API.Validators;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using FluentValidation.Results;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        private readonly IFileService _fileService;

        public EventsController(IEventsService eventsService, IFileService fileService)
        {
            _eventsService = eventsService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] GridifyQuery gridifyQuery)
        {
            var events = await _eventsService.GetEvents();
            var result = events.AsQueryable()
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequest createEventRequest)
        {
            CreateEventRequestValidator validator = new ();
            ValidationResult results = await validator.ValidateAsync(createEventRequest);

            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(createEventRequest.Image, allowedFileExtentions);
            var @event = Event.Create(createEventRequest.Title, createEventRequest.Description, 
                createEventRequest.DateTime, createEventRequest.Place, createEventRequest.Category, createEventRequest.MaxparticipantNumber,
                new List<Participant>(), createdImageName);
            var eventId = await _eventsService.CreateEvent(@event);
            return StatusCode(StatusCodes.Status201Created, eventId);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromForm] UpdateEventRequest updateEventRequest)
        {
            UpdateEventRequestValidator validator = new();
            ValidationResult results = await validator.ValidateAsync(updateEventRequest);

            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            var existingEvent = await _eventsService.GetEventById(id);
            if(existingEvent == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            string oldImage = existingEvent.Image;
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(updateEventRequest.Image, allowedFileExtentions);
            var eventId= await _eventsService.UpdateEvent(id, updateEventRequest.Title, updateEventRequest.Description,
                updateEventRequest.DateTime, updateEventRequest.Category, updateEventRequest.Place, updateEventRequest.MaxparticipantNumber, createdImageName);
            _fileService.DeleteFile(oldImage);
            return StatusCode(StatusCodes.Status200OK, eventId);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="Admin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _eventsService.GetEventById(id);
            await _eventsService.DeleteEvent(id);
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

        [HttpPost("unsubscribe")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromEvent(Guid eventId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            await _eventsService.UnsubscribeFromEvent(eventId, Guid.Parse(userId));
            return StatusCode(StatusCodes.Status200OK, "You unsubscribed from event");
        }
    }
}
