using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages
{
    public interface IMessageParser
    {
        Message Parse(string rawText);
    }
}
