using MassTransit;

namespace Events.Application.Dtos
{
    public class EventResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public string Place { get; set; } = string.Empty;
        public DateTime? DateTime { get; set; }
        public int MaxParticipantNumber { get; set; }
        public int? ParticipantsNumber { get; set; }
    }
}
