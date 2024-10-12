namespace Users.API.Dtos
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
