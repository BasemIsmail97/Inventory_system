

namespace Presistence.Repositories
{
    public class GenericRepository<TEntity>(InventoryDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task AddAsync(TEntity entity)
        => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
       => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        
         => asNoTracking
            ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() 
           : await _dbContext.Set<TEntity>().ToListAsync();

        

        public async Task<TEntity?> GetByIdAsync(int id)
       => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
       => _dbContext.Set<TEntity>().Update(entity);
        #region Specifications
        public async Task<int> CountAsync(ISpecification<TEntity> specifications)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specifications)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();


        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specifications)
       => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        #endregion
    }
}
