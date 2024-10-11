using Events.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Events.Domain.Interfaces.Repositories
{
    public interface IEventsRepository
    {
        Task<Guid> Create(Event @event);
        Task<Guid> Delete(Guid id);
        Task<List<Event>> Get();
        Task<Guid> Update(Guid id, string title, string description, DateTime dateTime, string category, string place, int maxParticipantsNumber, string image);
        Task<Event> GetById(Guid id);
        Task<Guid> AddParticipant(Guid id, Participant participant);
        Task RemoveParticipant(Guid eventId, Guid userId);
    }
}