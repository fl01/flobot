using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.Advanced)]
    [Message("PsychoRaid", "pr")]
    public class PsychoRaidHandler : MessageHandlerBase
    {
        public PsychoRaidHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            return new[] { ActivityBundle.Activity.CreateReply("...") };
        }

        protected override IEnumerable<Activity> CreateReplies()
        {

            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                return GetSubCommandReplies();
            }
            else
            {
                return GetCommandReplies();
            }
        }

        private IEnumerable<Activity> GetSubCommandReplies()
        {
            return Enumerable.Empty<Activity>();
        }

        private IEnumerable<Activity> GetCommandReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return new[] { ActivityBundle.Activity.CreateReply("Please provide character name!") };
            }

            return Enumerable.Empty<Activity>();
        }
    }
}
