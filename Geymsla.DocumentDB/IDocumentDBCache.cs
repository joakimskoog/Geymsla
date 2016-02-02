using System.Collections.Generic;

namespace Geymsla.DocumentDB
{
    public interface IDocumentDBCache<T>
    {
        /// <summary>
        /// Whether or not the cache is expired.
        /// </summary>
        bool IsExpired { get; }

        /// <summary>
        /// The number of items in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Retrieves the contents from the cache.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetContents();
    }
}