using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Flobot.Messages.Handlers;

namespace Flobot.Messages
{
    public interface IMessageHandlerProvider
    {
        IMessageHandler GetHandler();
    }
}
