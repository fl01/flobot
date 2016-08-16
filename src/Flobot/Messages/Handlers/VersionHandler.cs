using System;
using System.Collections.Generic;
using System.Reflection;
using Flobot.Common;
using Flobot.Identity;
using Humanizer;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("my version", Section.Default, "version", "ver")]
    public class VersionHandler : MessageHandlerBase
    {
        public VersionHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            // we do not have subcommands here, so, let's simply return error message if anything
            if (!string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                return GetInvalidSubCommandReply();
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            return new[] { ActivityBundle.Activity.CreateReply($"My version is {version.ToString()} and I was built {assembly.GetCompileDate().ToUniversalTime().Humanize()}") };
        }
    }
}