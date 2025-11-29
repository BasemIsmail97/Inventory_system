

namespace Presistence.Repositories
{
    public class UnitOfWork(InventoryDbContext context) : IUnitOfWork
    {
        private readonly InventoryDbContext _context = context;
        private ConcurrentDictionary<string, object> _repositories= new();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        => (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity>(_context));

        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
