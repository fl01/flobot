using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Flobot.Identity;
using Flobot.Messages.Handlers.Advice;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("advice", "adv")]
    public class AdviceHandler : MessageHandlerBase
    {
        private AdviceProvider adviceProvider;

        public AdviceHandler(User caller, Message message)
            : base(caller, message)
        {
            adviceProvider = new AdviceProvider();
        }

        protected override string GetReplyMessage(Activity activity)
        {
            return adviceProvider.GetAdvice();
        }
    }
}
