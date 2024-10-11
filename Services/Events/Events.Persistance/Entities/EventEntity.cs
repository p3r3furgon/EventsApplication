using Events.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Events.Persistance.Entities
{
    public class EventEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipantNumber { get; set; }
        public ICollection<ParticipantEntity> Participants { get; set; } = new List<ParticipantEntity>();
        public string? Image { get; set; } = string.Empty;
    }
}
