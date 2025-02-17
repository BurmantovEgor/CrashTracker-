using CrashTracker.Application.Log_s;
using CrashTracker.Core.Abstractions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CrashTracker.Core.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisLogService _logger;
        public CacheService(IDistributedCache cache, RedisLogService logger)
        {
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<Result<T>> GetCacheAsync<T>(string cacheKey)
        {
            try
            {
                var cachedDataStr = await _cache.GetStringAsync(cacheKey);
                if (string.IsNullOrEmpty(cachedDataStr)) return Result.Failure<T>("CacheIsEmpty");
              
                await _cache.RefreshAsync(cacheKey); 
                
                var cachedData = JsonSerializer.Deserialize<T>(cachedDataStr);
                return Result.Success<T>(cachedData);
            }
            catch (Exception ex)
            {
                _logger.LogRedisError("Ошибка доступа к Redis", ex);
                return Result.Failure<T>("CacheServiceNotAvailable");
            }
        }

        public async Task SetCacheAsync<T>(string cacheKey, T data, TimeSpan? expiration = null)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
                };

                await _cache.SetStringAsync(cacheKey, serializedData, options);
            }
            catch (Exception ex)
            {
                _logger.LogRedisError("Ошибка доступа к Redis", ex);
            }
        }

    }
}
