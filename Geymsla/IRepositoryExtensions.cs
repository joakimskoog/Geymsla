// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Geymsla.Collections;

#pragma warning disable 1573

namespace Geymsla
{
    /// <summary>
    /// Useful extension methods for use with <see cref="IReadOnlyRepository{T,TId}"/> and <see cref="IRepository{T,TId}"/>.
    /// </summary>
    public static class IRepositoryExtensions
    {
        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The list consisting of all items.</returns>
        public static async Task<IEnumerable<T>> GetAllAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetAllAsync(CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The list consisting of all items.</returns>
        public static async Task<IEnumerable<T>> GetAllAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetAsync(x => x, cancellationToken, includeProperties);
        }

        /// <summary>
        /// Retrieves all items, asynchronously, that satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The list consisting of all items that satisfies the given condition.</returns>
        public static async Task<IEnumerable<T>> FindByAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.FindByAsync(predicate, CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Retrieves all items, asynchronously, that satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The list consisting of all items that satisfies the given condition.</returns>
        public static async Task<IEnumerable<T>> FindByAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetAsync(x => x.Where(predicate), cancellationToken, includeProperties);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, or a default value if there are no items.
        /// </summary>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The first item, or a default value if there are no items.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetFirstOrDefaultAsync(CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, or a default value if there are no items.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The first item, or a default value if there are no items.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetFirstOrDefaultAsync((x => true), cancellationToken, includeProperties);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition-</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The first item that satisfies a the condition, or a default value if no such item is found.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetFirstOrDefaultAsync(predicate, CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition-</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The first item that satisfies a the condition, or a default value if no such item is found.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var items = await repository.GetAsync(x => x.Where(predicate), cancellationToken, includeProperties);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the only item, asynchronously, or a default value if there are no items. 
        /// This method throws an exception if there is more than one item.
        /// </summary>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The only item or a default value if there are no items.</returns>
        public static async Task<T> GetSingleOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetSingleOrDefaultAsync(CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Retrieves the only item, asynchronously, or a default value if there are no items.
        /// This method throws an exception if there is more than one item.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns>The only item or a default value if there are no items.</returns>
        public static async Task<T> GetSingleOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository,
            CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetSingleOrDefaultAsync((x => true), cancellationToken, includeProperties);
        }

        /// <summary>
        /// Returns the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// This method throws an exception if there are more than one items satisfied by the condition.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository,
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetSingleOrDefaultAsync(predicate, CancellationToken.None, includeProperties);
        }

        /// <summary>
        /// Returns the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// This method throws an exception if there are more than one items satisfied by the condition. 
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <param name="includeProperties">Specifies the related objects to include in the results.</param>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T, TId>(this IReadOnlyRepository<T, TId> repository,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var items = await repository.GetAsync(x => x.Where(predicate), cancellationToken, includeProperties);
            return items.SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="repository"></param>
        /// <param name="queryFilter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> GetPaginatedListAsync<T, TId>(this IReadOnlyRepository<T, TId> repository, Func<IQueryable<T>, IQueryable<T>> queryFilter, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            return await repository.GetPaginatedListAsync(queryFilter, pageNumber, pageSize, CancellationToken.None, includeProperties);
        }
    }
}