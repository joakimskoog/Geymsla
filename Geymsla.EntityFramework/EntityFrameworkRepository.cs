using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Geymsla.EntityFramework
{
    public class EntityFrameworkRepository<T,TId> : IRepository<T,TId> where T : class
    {
        protected readonly DbContext DbContext;

        public EntityFrameworkRepository(DbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            DbContext = dbContext;
        }

        public T Get(TId id)
        {
            return DbContext.Set<T>().Find((object)id);
        }

        public Task<T> GetAsync(TId id, CancellationToken cancellationToken)
        {
            return DbContext.Set<T>().FindAsync(cancellationToken, (object)id);
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryFilter)
        {
            var entities = GetAllAsQueryable();
            var filteredEntities = queryFilter(entities);

            return filteredEntities;
        }

        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken)
        {
            var entities = GetAllAsQueryable();
            var filteredEntities = queryFilter(entities);

            return await filteredEntities.ToArrayAsync(cancellationToken);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return DbContext.Set<T>();
        }
    }
}