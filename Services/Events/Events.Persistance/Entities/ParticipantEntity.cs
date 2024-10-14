namespace Events.Persistance.Entities
{
    public class ParticipantEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDateTime { get; set; } 
        public EventEntity? Event { get; set; }
    }
}
