using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers.Fuck
{
    public class FoaasResponse
    {
        public string Message { get; set; }

        public string Subtitle { get; set; }

        public override string ToString()
        {
            return string.Join(" ", Message, Subtitle);
        }
    }
}
