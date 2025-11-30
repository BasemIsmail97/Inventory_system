



namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly InventoryDbContext _context;
        private  ConcurrentDictionary<string, object> _repositories;
        private bool _disposed = false;

        public UnitOfWork(InventoryDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        => (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity>(_context));

        public async Task<int> SaveChangesAsync()
        {
            try
            {
              return  await _context.SaveChangesAsync();

            }
            catch(DbUpdateException ex)
            {
                throw new Exception("Error saving changes to database", ex);

            }
        }

        protected virtual void Dispose(bool disposing)
        {
           
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _context.Dispose();
                        _repositories.Clear();
                    }
                    _disposed = true;
                }
            
        }

       

        public void Dispose()
        {
            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
