using CommonFiles.Interfaces;

namespace Notifications.Persistance.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NotificationsDbContext _context;

        public UnitOfWork(NotificationsDbContext context)
        {
            _context = context;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
