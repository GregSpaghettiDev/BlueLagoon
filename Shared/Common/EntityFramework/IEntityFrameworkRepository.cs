using Common.EntityFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.EntityFramework
{
    public interface IEntityFrameworkRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetSingleOrDefaultWithoutTrackingAsync(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> GetListWithoutTrackingAsync(Expression<Func<TEntity, bool>> expression);
    }
}
