namespace Events.Domain.Models
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDateTime { get; set; }
        public Event? Event { get; set; }

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
