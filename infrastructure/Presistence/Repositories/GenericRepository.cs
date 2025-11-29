using Presistence.Data;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
