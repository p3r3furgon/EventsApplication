using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistance.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventsDbContext _context;
        public EventsRepository(EventsDbContext context)
        {
            _context = context;
        }
        public async Task Create(Event @event)
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> Get() =>
            await _context.Events.Include(e => e.Participants).ToListAsync();

        public async Task<Event> GetById(Guid id) => 
            await _context.Events.Include(e => e.Participants).Where(e => e.Id == id).FirstOrDefaultAsync();

        public async Task Delete(Guid id)
        {
            var @vent = await _context.Events.FindAsync(id);
            _context.Events.Remove(@vent);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Event @event)
        {

            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
        }

        public async Task AddParticipant(Event @event, Participant participant)
        {
            participant.Event = @event;
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveParticipant(Guid eventId, Guid userId)
        {
            var events = await _context.Events.Include(e => e.Participants).ToListAsync();
            var @event = events.Where(e => e.Id == eventId).FirstOrDefault();
            _context.RemoveRange(@event.Participants.Where(p => p.UserId == userId));
            await _context.SaveChangesAsync();
        }
    }
}
