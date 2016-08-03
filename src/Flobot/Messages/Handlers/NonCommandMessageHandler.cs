using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public class NonCommandMessageHandler : MessageHandlerBase
    {
        public NonCommandMessageHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            return new[] { ActivityBundle.Activity.CreateReply("...") };
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            return new[] { ActivityBundle.Activity.CreateReply($"Hello {ActivityBundle.Activity.From.Name}") };
        }
    }
}
