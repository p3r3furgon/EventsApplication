using Events.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Events.Domain.Interfaces.Services
{
    public interface IEventsService
    {
        Task<Guid> CreateEvent(Event @event);
        Task<Guid> DeleteEvent(Guid id);
        Task<List<Event>> GetAllEvents();
        Task<Event> GetEventById(Guid id);
        Task<string> RegisterUserOnEvent(Guid eventId, string firstName, string surname, string email, string userId);
        Task<string> UnsubscribeFromEvent(Guid eventId, Guid userId);
        Task<Guid> UpdateEvent(Guid id, string title, string description, DateTime dateTime, string category, string place, int maxParticipantsNumber, string image);
    }

}
