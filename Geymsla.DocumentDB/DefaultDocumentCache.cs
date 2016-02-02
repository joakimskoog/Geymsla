using System;
using System.Collections.Generic;

namespace Geymsla.DocumentDB
{
    public class OrderedSetDocumentDBCache<T> : IDocumentDBCache<T>
    {
        private DateTime _cacheDeath;
        private readonly TimeSpan _lifeSpan;
        private OrderedSet<T> _cache;

        public bool IsExpired => DateTime.UtcNow > _cacheDeath;
        public int Count => _cache.Count;

        public OrderedSetDocumentDBCache(TimeSpan lifeSpan)
        {
            _cache = new OrderedSet<T>();
            _lifeSpan = lifeSpan;
            _cacheDeath = DateTime.UtcNow + _lifeSpan;
        }

        public void Clear()
        {
            _cacheDeath = DateTime.UtcNow + _lifeSpan;
            _cache.Clear();
        }

        public void Add(T item)
        {
            _cache.Add(item);
        }

        public IEnumerable<T> GetContents()
        {
            return _cache;
        }
    }
}