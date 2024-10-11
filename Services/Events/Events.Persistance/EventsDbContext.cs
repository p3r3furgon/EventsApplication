using Events.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistance
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options)
        {
        }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<ParticipantEntity> Participants { get; set; }
    }
}
