using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Communication
{
    public class ExternalReply
    {
        public ReplyType Kind { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }
    }
}
