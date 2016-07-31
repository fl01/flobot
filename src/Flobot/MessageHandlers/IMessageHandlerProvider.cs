using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.MessageHandlers
{
    public interface IMessageHandlerProvider
    {
        IMessageHandler GetProvider(bool checkPermissions = true);
    }
}