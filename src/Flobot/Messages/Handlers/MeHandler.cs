using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("me")]
    public class MeHandler : MessageHandlerBase
    {
        public MeHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            return new[] { ActivityBundle.Activity.CreateReply("...") };
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine($"ID: {ActivityBundle.Caller.Id}");
            sb.AppendLine($"Name: {ActivityBundle.Caller.Name}");
            sb.AppendLine($"Role: {ActivityBundle.Caller.Role}");
            sb.AppendLine($"Group: {ActivityBundle.Caller.Group}");

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }
    }
}