using Users.Domain.Interfaces.Authentification;

namespace Users.Infrastructure
{
    public class PasswordHasher: IPasswordHasher
    {
        public string? Generate(string? password)
        {
            if (password == null)
                return null;
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }
        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
        }
    }
}
