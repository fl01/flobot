using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("My version", Section.Default, "version", "ver")]
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
                return new[] { ActivityBundle.Activity.CreateReply(UnknownSubCommandError) };
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            double elapsedFromBuildDate = Math.Round((DateTime.Now - assembly.GetCompileDate()).TotalHours, SettingsService.GetElapsedHoursFromBuildDateRound());

            return new[] { ActivityBundle.Activity.CreateReply($"My version is {version.ToString()} and I was built {elapsedFromBuildDate} hour(s) ago") };
        }
    }
}