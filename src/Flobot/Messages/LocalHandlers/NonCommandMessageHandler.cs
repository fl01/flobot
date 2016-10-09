using System.Collections.Generic;
using Flobot.Common;
using Flobot.Eliza;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    public class NonCommandMessageHandler : MessageHandlerBase
    {
        private static ElizaMain eliza = new ElizaMain();

        protected override IEnumerable<Activity> CreateHelpReplies(ActivityBundle activityBundle)
        {
            return CreateSingleReplyCollection(activityBundle, "...");
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            string reply = eliza.ProcessInput(activityBundle.Message.RawText);

            return CreateSingleReplyCollection(activityBundle, reply);
        }
    }
}
