namespace Events.Domain.Models
{
    public class Participant
    {
        public Guid Id { get; }
        public Guid UserId { get; set; }
        public string FirstName { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public ICollection<Event> Events { get; set; } = new List<Event>();

        private Participant(Guid id, Guid userId, string firstName, string surname, string email)
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            Surname = surname;
            Email = email;
        }

        public static Participant Create(Guid userId, string firstName, string surname, string email)
        {
            return new Participant(Guid.NewGuid(), userId, firstName, surname, email);
        }
    }

}
