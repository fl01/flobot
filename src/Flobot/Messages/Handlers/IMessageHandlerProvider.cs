using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;

namespace Flobot.Messages.Handlers
{
    public interface IMessageHandlerProvider
    {
        IMessageHandler GetHandler(Message message);
    }
}
