namespace Users.Persistance.Entities
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
    }
}
