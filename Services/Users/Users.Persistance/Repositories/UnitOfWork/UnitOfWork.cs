using CommonFiles.Interfaces;

namespace Users.Persistance.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext _context;

        public UnitOfWork(UsersDbContext context)
        {
            _context = context;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync();
        }
    }
}
