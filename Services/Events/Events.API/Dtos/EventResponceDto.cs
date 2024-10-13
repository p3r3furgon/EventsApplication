namespace Events.API.Dtos
{
    public class EventResponceDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public int MaxParticipantNumber { get; set; }
        public int ParticipantsNumber { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
