using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Geymsla.Collections;

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
        /// 
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<IPagedList<T>> GetPaginatedListAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, int pageNumber, int pageSize, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetAllAsQueryable();
    }
}