// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 1573

namespace Geymsla
{
    /// <summary>
    /// Useful extension methods for use with <see cref="IReadOnlyRepository{T}"/> and <see cref="IRepository{T}"/>.
    /// </summary>
    public static class IRepositoryExtensions
    {
        /// <summary>
        /// Retrieves all items in the repository.
        /// </summary>
        /// <returns>The list consisting of all items.</returns>
        public static IEnumerable<T> GetAll<T>(this IReadOnlyRepository<T> repository) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return repository.Get(x => x);
        }

        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <returns>The list consisting of all items.</returns>
        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IReadOnlyRepository<T> repository) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetAllAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The list consisting of all items.</returns>
        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetAsync(x => x, cancellationToken);
        }

        /// <summary>
        /// Retrieves all items, asynchronously, that satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <returns>The list consisting of all items that satisfies the given condition.</returns>
        public static async Task<IEnumerable<T>> FindByAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.FindByAsync(predicate, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves all items, asynchronously, that satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The list consisting of all items that satisfies the given condition.</returns>
        public static async Task<IEnumerable<T>> FindByAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetAsync(x => x.Where(predicate), cancellationToken);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, or a default value if there are no items.
        /// </summary>
        /// <returns>The first item, or a default value if there are no items.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetFirstOrDefaultAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, or a default value if there are no items.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The first item, or a default value if there are no items.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetFirstOrDefaultAsync((x => true), cancellationToken);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition-</param>
        /// <returns>The first item that satisfies a the condition, or a default value if no such item is found.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository,
            Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetFirstOrDefaultAsync(predicate, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the first item, asynchronously, that satisfies a condition or a default value if no such item is found.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition-</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The first item that satisfies a the condition, or a default value if no such item is found.</returns>
        public static async Task<T> GetFirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var items = await repository.GetAsync(x => x.Where(predicate), cancellationToken);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetSingleOrDefaultAsync(CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            return await repository.GetSingleOrDefaultAsync((x => true), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository,
            Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await repository.GetSingleOrDefaultAsync(predicate, CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> GetSingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var items = await repository.GetAsync(x => x.Where(predicate), cancellationToken);
            return items.SingleOrDefault();
        }
    }
}