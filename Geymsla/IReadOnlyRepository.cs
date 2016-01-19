﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Geymsla
{
    /// <summary>
    /// Interface that provides methods for reading items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(object id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken);
    }
}