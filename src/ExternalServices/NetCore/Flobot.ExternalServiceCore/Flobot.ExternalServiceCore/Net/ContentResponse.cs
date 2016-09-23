using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Net
{
    public class ContentResponse
    {
        public Uri ResponseUrl { get; set; }

        public string Content { get; set; }
    }
}
