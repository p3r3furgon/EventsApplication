using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories
{
    public interface IEventsRepository
    {
        Task Create(Event @event);
        Task Delete(Guid id);
        Task<List<Event>> Get();
        Task Update(Event @event);
        Task<Event> GetById(Guid id);
        Task AddParticipant(Event @event, Participant participant);
        Task RemoveParticipant(Guid eventId, Guid userId);
    }
}