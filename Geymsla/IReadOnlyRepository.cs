using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Geymsla
{
    public interface IReadOnlyRepository<T>
    {
        IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> filter);
        T Get(Func<IQueryable<T>, T> filter);

        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> filter, CancellationToken cancellationToken);
        Task<T> GetAsync(Func<IQueryable<T>, T> filter, CancellationToken cancellationToken);
    }
}