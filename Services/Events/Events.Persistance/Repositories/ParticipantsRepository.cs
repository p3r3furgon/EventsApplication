using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistance.Repositories
{
    public class ParticipantsRepository : IParticipantsRepository
    {
        private EventsDbContext _context;

        public ParticipantsRepository(EventsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Participant>> GetByEventId(Guid eventId, int pageNumber, int pageSize) =>
            await _context.Participants
            .Where(p => p.Event.Id == eventId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task Add(Participant participant) =>
            await _context.Participants.AddAsync(participant);

        public async Task DeleteByUserId(Guid userId)
        {
            var participant = await _context.Participants
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();
            _context.Participants.Remove(participant);
        }
    }
}
