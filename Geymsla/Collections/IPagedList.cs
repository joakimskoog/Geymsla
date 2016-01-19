using System;
using System.Collections.Generic;

namespace Geymsla.Collections
{
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// One-based index of this page in the superset.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Null if this page is the first one in the superset, otherwise it's the previous page's number.
        /// </summary>
        int? PreviousPageNumber { get; }

        /// <summary>
        /// Null if this page is the last one in the superset, otherwise it's the next page's number.
        /// </summary>
        int? NextPageNumber { get; }

        /// <summary>
        /// The number of the first page in the superset.
        /// </summary>
        int FirstPageNumber { get; }

        /// <summary>
        /// The number of the last page in the superset.
        /// </summary>
        int LastPageNumber { get; }

        /// <summary>
        /// The maximum size of any page in the superset.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Total number of pages in the superset.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Converts the IPagedList{T} to an IPagedList{TOutput}
        /// </summary>
        /// <typeparam name="TOutput">The type that the new IPagedList should consist of.</typeparam>
        /// <param name="converter">The converter that converts items of type T to type TOutput.</param>
        /// <returns>The newly created IPagedList{TOutput}</returns>
        IPagedList<TOutput> ConvertTo<TOutput>(Converter<T, TOutput> converter);
    }
}