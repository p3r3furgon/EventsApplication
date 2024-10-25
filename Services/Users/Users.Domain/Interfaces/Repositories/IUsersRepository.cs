using Users.Domain.Models;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<List<User>> Get();
        Task Create(User user);
        Task Delete(Guid id);
        Task Update(User user);
        Task<User> GetByEmail(string email);
        Task<User> GetById(Guid id);
    }
}
