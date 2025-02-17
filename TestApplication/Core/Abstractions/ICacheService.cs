using CSharpFunctionalExtensions;

namespace CrashTracker.Core.Abstractions
{
    public interface ICacheService
    {
        public Task<Result<T>> GetCacheAsync<T>(string cacheKey);
        public Task SetCacheAsync<T>(string cacheKey, T data, TimeSpan? expiration = null);
    }
}
