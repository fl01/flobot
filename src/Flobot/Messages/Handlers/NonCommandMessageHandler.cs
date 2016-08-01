using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public class NonCommandMessageHandler : MessageHandlerBase
    {
        public NonCommandMessageHandler(User caller, Message message)
            : base(caller, message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            return $"Hello {activity.From.Name}";
        }
    }
}
