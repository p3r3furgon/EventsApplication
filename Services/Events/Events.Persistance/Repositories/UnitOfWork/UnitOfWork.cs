using CommonFiles.Interfaces;

namespace Events.Persistance.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsDbContext _context;

        public UnitOfWork(EventsDbContext context)
        {
            _context = context;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync();
        }
    }
}
