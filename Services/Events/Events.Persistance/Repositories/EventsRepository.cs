using AutoMapper;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistance.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistance.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventsDbContext _context;
        private readonly IMapper _mapper;
        public EventsRepository(EventsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Guid> Create(Event @event)
        {
            var eventEntity = _mapper.Map<Event, EventEntity>(@event);
            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();
            return @event.Id;
        }

        public async Task<List<Event>> Get()
        {
            var eventsEntities = await _context.Events.Include(e => e.Participants).ToListAsync();
            var events = _mapper.Map<List<EventEntity>, List<Event>>(eventsEntities);

            return events;
        }

        public async Task<Event> GetById(Guid id)
        {
            var eventEntity = await _context.Events.Include(e => e.Participants).Where(e => e.Id == id).FirstOrDefaultAsync();
            if (eventEntity == null)
                throw new Exception("Event is not found");
            var @event = _mapper.Map<EventEntity, Event>(eventEntity);
            return @event;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Events
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }

        public async Task<Guid> Update(Guid id, string title, string description, DateTime dateTime, string category, string place, int maxParticipantsNumber, string image)
        {
            await _context.Events
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.Title, p => title)
                    .SetProperty(p => p.Description, p => description)
                    .SetProperty(p => p.DateTime, p => dateTime)
                    .SetProperty(p => p.Category, p => category)
                    .SetProperty(p => p.Place, p => place)
                    .SetProperty(p => p.Image, p => image)
                    .SetProperty(p => p.MaxParticipantNumber, p => maxParticipantsNumber));
            return id;
        }

        public async Task<Guid> AddParticipant(Guid id, Participant participant)
        {
            var eventsEntities = await _context.Events.ToListAsync();
            var eventEntity = eventsEntities.Where(e => e.Id == id).FirstOrDefault();
            if (eventEntity == null)
                throw new Exception("Event is not found");
            var participantEntity = _mapper.Map<ParticipantEntity>(participant);
            await _context.Participants.AddAsync(participantEntity);
            eventEntity.Participants.Add(participantEntity);
            await _context.SaveChangesAsync();
            return eventEntity.Id;

        }

        public async Task RemoveParticipant(Guid eventId, Guid userId)
        {
            var eventsEntities = await _context.Events.Include(e => e.Participants).ToListAsync();
            var eventEntity = eventsEntities.Where(e => e.Id == eventId).FirstOrDefault();
            if (eventEntity == null)
                throw new Exception("Event is not found");
            _context.RemoveRange(eventEntity.Participants.Where(p => p.UserId == userId));
            await _context.SaveChangesAsync();
        }
    }
}
