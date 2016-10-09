using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flobot.Common.Cache
{
    public interface ICacheStorage
    {
        bool Contains(string key);

        bool Put<T>(CacheObject<T> obj);

        CacheObject<T> Get<T>(string key);
    }
}
