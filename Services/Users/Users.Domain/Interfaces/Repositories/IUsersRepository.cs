using Users.Domain.Models;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<ICollection<User>> Get();
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string firstName, string surname, DateTime birthDate, string email, string passwordHash, string role);
        Task<User> GetByEmail(string email);
    }
}
