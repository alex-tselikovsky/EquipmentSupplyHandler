using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESHRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace ESHRepository.EF
{
    public class EFGenericRepository<TEntity> : ICRUDRepository<TEntity> where TEntity: GUIDEntity , new()
    {
        DbContext _context;
        protected DbSet<TEntity> _dbSet;

        public EFGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync();
        }

        protected virtual IQueryable<TEntity> Query => _dbSet.AsNoTracking();

        public Task<TEntity> GetAsync(string guid)
        {
            return Query.FirstOrDefaultAsync(t=>t.Id == guid);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToArrayAsync();

        public Task RemoveAsync(string guid)
        {
            TEntity entity = new TEntity() { Id = guid };
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}