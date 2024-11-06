namespace CommonFiles.Interfaces
{
    public interface IUnitOfWork
    {
        public Task Save(CancellationToken cancellationToken);
    }
}
