using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public class NonCommandMessageHandler : MessageHandlerBase
    {
        public NonCommandMessageHandler(Message message)
            : base(message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            return $"Hello {activity.From.Name}";
        }
    }
}
