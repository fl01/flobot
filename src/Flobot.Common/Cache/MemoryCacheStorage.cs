using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Flobot.Common.Cache
{
    public class MemoryCacheStorage : ICacheStorage
    {
        private MemoryCache cache;

        public MemoryCacheStorage()
        {
            var config = new NameValueCollection()
            {
                { "PollingInterval", TimeSpan.FromMinutes(1).ToString() }
            };

            cache = new MemoryCache(Guid.NewGuid().ToString(), config);
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        public CacheObject<T> Get<T>(string key)
        {
            return cache.Get(key) as CacheObject<T>;
        }

        public bool Put<T>(CacheObject<T> obj)
        {
            return cache.Add(obj.Name, obj.Value, obj.Expiry);
        }
    }
}
