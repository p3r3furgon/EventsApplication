using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Gridify;
using Microsoft.AspNetCore.Http;

namespace Events.Application.services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IFileService _fileService;
        public EventsService(IEventsRepository eventsRepository, IFileService fileService)
        {
            _eventsRepository = eventsRepository;
            _fileService = fileService;
        }

        public async Task<List<Event>> GetEvents()
        {
            return await _eventsRepository.Get();
        }
        public async Task<Guid> CreateEvent(Event @event)
        {

            return await _eventsRepository.Create(@event);
        }

        public async Task<Guid> DeleteEvent(Guid id)
        {
            return await _eventsRepository.Delete(id);
        }
        public async Task<Guid> UpdateEvent(Guid id, string? title, string? description, DateTime? dateTime, 
            string? category, string? place, int? maxParticipantsNumber, string? image)
        {
            return await _eventsRepository.Update(id, title, description, dateTime, category, place, maxParticipantsNumber, image);
        }

        public async Task RegisterUserOnEvent(Guid eventId, string firstName, string surname, string email, string userId)
        {
            var @event = await _eventsRepository.GetById(eventId);

            if (@event.Participants.Count >= @event.MaxParticipantNumber)
                throw new Exception("There are no free spots in this event");
            if (@event.Participants.Where(p => p.UserId == Guid.Parse(userId)).FirstOrDefault() != null)
                throw new Exception("You are already registered on this event");

            var participant = Participant.Create(Guid.Parse(userId), firstName, surname, email, DateTime.UtcNow);

            await _eventsRepository.AddParticipant(eventId, participant);
        }

        public async Task UnsubscribeFromEvent(Guid eventId, Guid userId)
        {

            var @event = await _eventsRepository.GetById(eventId);
            if (@event == null)
                throw new Exception("Event is not found");
            var participant = @event.Participants.Where(p => p.UserId == userId).FirstOrDefault();
            if (participant == null)
                throw new Exception("You are not subscribed on this event");

            await _eventsRepository.RemoveParticipant(eventId, userId);
        }

        public Task<Event> GetEventById(Guid id)
        {
            return _eventsRepository.GetById(id);
        }

    }
}
