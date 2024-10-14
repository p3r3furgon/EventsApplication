namespace Events.API.Dtos
{
    public class ParticipantResponceDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDateTime { get; set; }

    }
}
