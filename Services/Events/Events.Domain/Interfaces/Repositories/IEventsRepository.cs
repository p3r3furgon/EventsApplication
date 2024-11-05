using Events.Domain.Models;
using Gridify;

namespace Events.Domain.Interfaces.Repositories
{
    public interface IEventsRepository
    {
        Task Create(Event @event);
        Task Delete(Guid id);
        Task<List<Event>> Get(int pageNumber, int pageSize, string? filter);
        void Update(Event @event);
        Task<Event?> GetById(Guid id);
        Task AddParticipant(Event @event, Participant participant);
        Task RemoveParticipant(Guid eventId, Guid userId);
    }
}