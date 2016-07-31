using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Flobot.MessageHandlers
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        public IMessageHandler GetProvider(bool checkPermissions = true)
        {
            return null;
        }
    }
}