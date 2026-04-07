using Library.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Library.API.Services
{
    public class CacheService: ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly ConcurrentDictionary<string, DateTime> _accessTimes = new();

        private int _hits = 0;
        private int _misses = 0;
        private int _evictions = 0;
        public CacheService(IMemoryCache cache, ILogger<CacheService> logger) {
            _cache = cache;
            _logger = logger;
        }

        public T? Get<T>(string key)
        {
            if( _cache.TryGetValue(key, out T? value))
            {
                _hits++;
                _accessTimes[key] = DateTime.UtcNow;
                return value;
            }
            else
            {
                _misses++;
                return default;
            }
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1,
                Priority = CacheItemPriority.Normal,
            };

            options.RegisterPostEvictionCallback((k, v, reason, state) =>
            {
                _evictions++;
                _logger.LogInformation("Cache item with key '{Key}' was evicted due to {Reason}.", k, reason);
            });

            if(_evictions > 1000)
            {
                _logger.LogWarning("Cache eviction count has exceeded 1000. Consider reviewing cache usage patterns.");
            }

            _cache.Set(key, value, options);
            _accessTimes[key] = DateTime.UtcNow;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _accessTimes.TryRemove(key, out _);
            _logger.LogInformation("Cache item with key '{Key}' was removed.", key);
        }

        public bool Exists(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public CacheHealth GetHealth()
        {
            return new CacheHealth
            {
                Hits = _hits,
                Misses = _misses,
                Evictions = _evictions,
                CurrentSize = _accessTimes.Count
            };
        }

        public class CacheHealth
        {
            public int Hits { get; set; }
            public int Misses { get; set; }
            public int Evictions { get; set; }
            public int CurrentSize { get; set; }
        }
    }
}
