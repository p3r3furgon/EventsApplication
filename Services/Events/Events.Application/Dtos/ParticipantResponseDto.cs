namespace Events.Application.Dtos
{
    public class ParticipantResponseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDateTime { get; set; }
    }

}
