using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Gridify;
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
        }

        public async Task<List<Event>> Get(int pageNumber, int pageSize, string? filter) =>
           await _context.Events
            .Include(e => e.Participants)
            .OrderBy(e => e.DateTime)
            .ApplyFiltering(filter)
            .ApplyPaging(pageNumber, pageSize)
            .ToListAsync();


        public async Task<Event?> GetById(Guid id) =>
            await _context.Events
            .Include(e => e.Participants)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();

        public async Task Delete(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
        }

        public void Update(Event @event) => 
            _context.Events.Update(@event);

        public async Task AddParticipant(Event @event, Participant participant)
        {
            participant.Event = @event;
            await _context.Participants.AddAsync(participant);
        }

        public async Task RemoveParticipant(Guid eventId, Guid userId)
        {
            var events = await _context.Events.Include(e => e.Participants).ToListAsync();
            var @event = events.Where(e => e.Id == eventId).FirstOrDefault();
            _context.RemoveRange(@event.Participants.Where(p => p.UserId == userId));
        }
    }
}
