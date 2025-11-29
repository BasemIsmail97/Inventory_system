using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class UnitOfWork(InventoryDbContext context) : IUnitOfWork
    {
        private readonly InventoryDbContext _context = context;
        public ConcurrentDictionary<string, object> _repositories= new();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        => (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity>(_context));

        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
