
using System.Runtime.Caching;

namespace OnlineShopAPIFull.Services.Caching
{
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;
      
        public T GetData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;

            return (T)_memoryCache.Get(key);
        }

        public object RemoveData(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            _memoryCache.Remove(key);
            return true;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            _memoryCache.Set(key, value, expirationTime);

            return true;
        }
    }
}
