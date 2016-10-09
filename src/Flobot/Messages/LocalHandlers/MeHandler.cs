using System.Collections.Generic;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("know yourself", Section.Default, "me")]
    public class MeHandler : MessageHandlerBase
    {
        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine($"ID: {activityBundle.Caller.Id}");
            sb.AppendLine($"Name: {activityBundle.Caller.Name}");
            sb.AppendLine($"Role: {activityBundle.Caller.Role}");
            sb.AppendLine($"Group: {activityBundle.Caller.Group}");

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }
    }
}