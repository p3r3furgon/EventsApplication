namespace Users.API.Dtos
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
