namespace Users.API.Dtos
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
