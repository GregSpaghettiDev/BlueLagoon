using Common.EntityFramework.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.EntityFramework
{
    public abstract class EntityFrameworkRepository<TEntity> : IEntityFrameworkRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected DbContext Context { get; }

        protected DbSet<TEntity> Set { get; }

        protected EntityFrameworkRepository(DbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        public Task AddAsync(IEnumerable<TEntity> entities) => Set.AddRangeAsync(entities);

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entities = Set.AsNoTracking().Where(expression);
            Set.RemoveRange(entities);
        }

        public Task<TEntity> GetSingleOrDefaultWithoutTrackingAsync(Expression<Func<TEntity, bool>> expression) => Set.AsNoTracking().SingleOrDefaultAsync(expression);

        public Task<List<TEntity>> GetListWithoutTrackingAsync(Expression<Func<TEntity, bool>> expression) => Set.AsNoTracking().Where(expression).ToListAsync();
    }
}
