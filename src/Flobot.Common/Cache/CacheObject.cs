using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flobot.Common.Cache
{
    public class CacheObject<T>
    {
        public T Value { get; set; }

        public string Name { get; set; }

        public DateTime Expiry { get; set; }
    }
}
