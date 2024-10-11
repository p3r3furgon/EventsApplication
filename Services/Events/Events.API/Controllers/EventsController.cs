using Events.API.Dtos;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
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
        public async Task<ICollection<Event>> GetAllEvents()
        {
            return await _eventsService.GetAllEvents();
        }

        [HttpGet("{id}")]
        public async Task<Event> GetEventById(Guid id)
        {
            return await _eventsService.GetEventById(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<Guid> CreateEvent([FromForm] CreateEventRequest createEventRequest)
        {
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(createEventRequest.Image, allowedFileExtentions);
            var @event = Event.Create(createEventRequest.Title, createEventRequest.Description, 
                createEventRequest.DateTime, createEventRequest.Place, createEventRequest.Category, createEventRequest.MaxparticipantNumber,
                new List<Participant>(), createdImageName);
            return await _eventsService.CreateEvent(@event);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<Guid> UpdateEvent(Guid id, [FromForm] UpdateEventRequest updateEventRequest)
        {
            var existingEvent = await _eventsService.GetEventById(id);
            string oldImage = existingEvent.Image;
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(updateEventRequest.Image, allowedFileExtentions);
            var eventId= await _eventsService.UpdateEvent(id, updateEventRequest.Title, updateEventRequest.Description,
                updateEventRequest.DateTime, updateEventRequest.Category, updateEventRequest.Place, updateEventRequest.MaxparticipantNumber, createdImageName);
            _fileService.DeleteFile(oldImage);
            return eventId;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="Admin")]
        public async Task<Guid> DeleteEvent(Guid id)
        {
            var @event = await _eventsService.GetEventById(id);
            await _eventsService.DeleteEvent(id);
            _fileService.DeleteFile(@event.Image);
            return @event.Id;
        }

        [HttpPost("subscribe")]
        [Authorize]
        public  async Task<string> RegisterUserOnEvent(Guid eventId)
        {
            var firstName = User?.FindFirstValue(ClaimTypes.Name);
            var surname = User?.FindFirstValue(ClaimTypes.Surname);
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            return await _eventsService.RegisterUserOnEvent(eventId, firstName, surname, email, userId);
        }

        [HttpPost("unsubscribe")]
        [Authorize]
        public async Task<string> UnsubscribeFromEvent(Guid eventId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            return await _eventsService.UnsubscribeFromEvent(eventId, Guid.Parse(userId));
        }
    }
}
