namespace Events.Domain.Models
{
    public class Participant
    {
        public Guid Id { get; }
        public Guid UserId { get; set; }
        public string FirstName { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public DateTime RegistrationDateTime { get; }
        public Event? Events { get; set; }

        private Participant(Guid id, Guid userId, string firstName, string surname, string email, DateTime registrationDateTime)
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            Surname = surname;
            Email = email;
            RegistrationDateTime = registrationDateTime;
        }

        public static Participant Create(Guid userId, string firstName, string surname, string email, DateTime registrationDateTime)
        {
            return new Participant(Guid.NewGuid(), userId, firstName, surname, email, registrationDateTime);
        }
    }

}
