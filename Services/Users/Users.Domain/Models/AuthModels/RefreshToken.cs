namespace Users.Domain.Models.AuthModels
{
    public class RefreshToken
    {
        public RefreshToken(Guid id, string token, string userEmail, DateTime expirationDate)
        {
            Id = id;
            Token = token;
            UserEmail = userEmail;
            ExpirationDate = expirationDate;
        }
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
    }
}
