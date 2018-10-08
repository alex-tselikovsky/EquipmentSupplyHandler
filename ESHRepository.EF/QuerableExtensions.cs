using System;
using System.Linq;
using System.Linq.Expressions;

namespace ESHRepository.EF
{
    public static class QuerableExtensions
    {
        public static  IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
