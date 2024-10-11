namespace Events.Persistance.Entities
{
    public class ParticipantEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
    }
}
