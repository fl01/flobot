using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Eliza;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public class NonCommandMessageHandler : MessageHandlerBase
    {
        private static ElizaMain eliza = new ElizaMain();

        public NonCommandMessageHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            return CreateSingleReplyCollection("...");
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            string reply = eliza.ProcessInput(ActivityBundle.Message.RawText);

            return CreateSingleReplyCollection(reply);
        }
    }
}
