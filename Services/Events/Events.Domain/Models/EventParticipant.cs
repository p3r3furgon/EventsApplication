namespace Events.Domain.Models
{
    public class EventParticipant
    {
        public Guid EventId { get; }
        public Guid ParticipantId { get; }
        public DateTime RegistrationDateTime { get; }
    }
}
