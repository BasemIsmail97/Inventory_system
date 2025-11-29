



namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _context;
        private ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(InventoryDbContext context)
        {
            _context = context;
            _repositories = new();
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        => (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity>(_context));

        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
