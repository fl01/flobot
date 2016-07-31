using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers
{
    public interface IMessageHandler
    {
        Activity CreateReply(Activity activity);
    }
}
