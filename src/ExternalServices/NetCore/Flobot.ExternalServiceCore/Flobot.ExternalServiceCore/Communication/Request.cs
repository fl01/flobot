using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Communication
{
    public class Request
    {
        public Message Message { get; set; }

        public Caller Caller { get; set; }
    }
}
