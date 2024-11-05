using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories
{
    public interface IParticipantsRepository
    {
        Task<List<Participant>> GetByEventId(Guid eventId, int pageNumber, int pageSize);
        Task Add(Participant participant);
        Task DeleteByUserId(Guid userId);
    }
}
