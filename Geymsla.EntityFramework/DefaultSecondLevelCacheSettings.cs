using System;

namespace Geymsla.EntityFramework
{
    public class DefaultSecondLevelCacheSettings : ISecondLevelCacheSettings
    {
        public bool ShouldUseSecondLevelCache { get; }
        public TimeSpan CacheLifeSpan { get; }

        public DefaultSecondLevelCacheSettings(bool shouldUseSecondLevelCache, TimeSpan cacheLifeSpan)
        {
            ShouldUseSecondLevelCache = shouldUseSecondLevelCache;
            CacheLifeSpan = cacheLifeSpan;
        }
    }
}