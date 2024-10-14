using Events.Domain.Models;
using Gridify;

namespace Events.Domain.Interfaces.Services
{
    public interface IEventsService
    {
        Task<Guid> CreateEvent(Event @event);
        Task<Guid> DeleteEvent(Guid id);
        Task<List<Event>> GetEvents();
        Task<Event> GetEventById(Guid id);
        Task RegisterUserOnEvent(Guid eventId, string firstName, string surname, string email, string userId);
        Task UnsubscribeFromEvent(Guid eventId, Guid userId);
        Task<Guid> UpdateEvent(Guid id, string? title, string? description, DateTime? dateTime, string? category, string? place, int? maxParticipantsNumber, string? image);
    }

}
