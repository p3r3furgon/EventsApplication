using System.Security.Principal;
using Users.Domain.Models;

namespace Users.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task<Guid> DeleteUser(Guid id);
        Task<ICollection<User>> GetUsers();
        Task<Guid> UpdateUser(Guid id, string firstName, string surname, DateOnly? birthDate, string email, string password, string role);
    }
}
