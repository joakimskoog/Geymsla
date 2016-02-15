using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EFSecondLevelCache;
using EFSecondLevelCache.Contracts;
using Geymsla.Collections;

namespace Geymsla.EntityFramework
{
    public class EntityFrameworkRepository<T, TId> : IRepository<T, TId> where T : class
    {
        protected readonly DbContext DbContext;
        private readonly ISecondLevelCacheSettings _secondLevelCacheSettings;

        public EntityFrameworkRepository(DbContext dbContext, ISecondLevelCacheSettings secondLevelCacheSettings)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            if (secondLevelCacheSettings == null) throw new ArgumentNullException(nameof(secondLevelCacheSettings));
            DbContext = dbContext;
            _secondLevelCacheSettings = secondLevelCacheSettings;
        }

        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            var entities = GetAllAsQueryable();
            var filteredEntities = queryFilter(entities);

            filteredEntities = filteredEntities.IncludeMultiple(includeProperties);

            if (_secondLevelCacheSettings.ShouldUseSecondLevelCache)
            {
                filteredEntities = filteredEntities.Cacheable(new EFCachePolicy { AbsoluteExpiration = DateTime.UtcNow + _secondLevelCacheSettings.CacheLifeSpan });
            }

            return await filteredEntities.ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<T>> GetPaginatedListAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter,
            int pageNumber, int pageSize, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            var count = queryFilter(GetAllAsQueryable()).Count();
            var paginationData = new PaginationData(count, pageNumber, pageSize);

            Func<IQueryable<T>, IQueryable<T>> paginationFunc = x =>
            {
                var query = queryFilter(x);
                return query.Skip((paginationData.PageNumber - 1) * paginationData.PageSize).Take(paginationData.PageSize);
            };

            var items = await GetAsync(paginationFunc, cancellationToken, includeProperties);
            return new PagedList<T>(paginationData, items);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return DbContext.Set<T>();
        }
    }
}