using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Geymsla
{
    /// <summary>
    /// Interface that provides methods for reading items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IReadOnlyRepository<T, in TId> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Retrieves all items as a queryable.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllAsQueryable(params Expression<Func<T,object>>[] includeProperties);
    }
}