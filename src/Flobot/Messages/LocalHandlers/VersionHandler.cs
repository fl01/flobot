using System;
using System.Collections.Generic;
using System.Reflection;
using Flobot.Common;
using Flobot.Identity;
using Humanizer;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("my version", Section.Default, "version", "ver")]
    public class VersionHandler : MessageHandlerBase
    {
        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            // we do not have subcommands here, so, let's simply return error message if anything
            if (!string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                return GetInvalidSubCommandReply(activityBundle);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            return CreateSingleReplyCollection(activityBundle, $"My version is {version.ToString()} and I was built {assembly.GetCompileDate().ToUniversalTime().Humanize()}");
        }
    }
}