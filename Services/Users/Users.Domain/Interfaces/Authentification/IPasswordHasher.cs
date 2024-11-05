namespace Users.Domain.Interfaces.Authentification
{
    public interface IPasswordHasher
    {
        string? Generate(string? password);
        bool Verify(string password, string passwordHash);
    }
}
