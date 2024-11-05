using Users.Domain.Models;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<List<User>> Get(int pageNumber, int PageSize);
        Task Create(User user);
        Task Delete(Guid id);
        void Update(User user);
        Task<User> GetByEmail(string email);
        Task<User> GetById(Guid id);
    }
}
