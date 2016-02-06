using System;

namespace Geymsla.EntityFramework
{
    public interface ISecondLevelCacheSettings
    {
        bool ShouldUseSecondLevelCache { get; }
        TimeSpan CacheLifeSpan { get; } 
    }
}