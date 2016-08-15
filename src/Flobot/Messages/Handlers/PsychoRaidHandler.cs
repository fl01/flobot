using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Handlers.PsychoRaid;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User, Group.PsychoRaid)]
    [Message(Section.Default, "PsychoRaid", "pr")]
    public class PsychoRaidHandler : MessageHandlerBase
    {
        private GoogleDocProxy proxy;

        private GoogleDocProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = IoC.Container.Resolve<GoogleDocProxy>();
                }

                return proxy;
            }
        }

        private Dictionary<string, Func<IEnumerable<Activity>>> subCommands;

        public PsychoRaidHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            subCommands = new Dictionary<string, Func<IEnumerable<Activity>>>()
            {
                {"all", GetAllMembers }
            };
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var key in subCommands.Select(x => x.Key))
            {
                sb.AppendLine("!PsychoRaid." + key);
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (!string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
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
            Func<IEnumerable<Activity>> subCommand;

            if (!subCommands.TryGetValue(ActivityBundle.Message.SubCommand, out subCommand))
            {
                return new[] { ActivityBundle.Activity.CreateReply("Unknown subcommand. Try use /? to see a list of available commands") };
            }

            return subCommand();
        }

        private IEnumerable<Activity> GetAllMembers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            IEnumerable<RaidMember> allmembers = Proxy.GetAllRaidMembers();

            foreach (var member in allmembers)
            {
                sb.AppendLine($"Member: {member.Name}, Sum: {member.Sum}");
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        private IEnumerable<Activity> GetCommandReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return new[] { ActivityBundle.Activity.CreateReply("Character name is required.") };
            }

            IEnumerable<RaidMember> members = Proxy.GetRaidMembers(ActivityBundle.Message.CommandArg);

            if (members == null || !members.Any())
            {
                return new[] { ActivityBundle.Activity.CreateReply($"Member '{ActivityBundle.Message.CommandArg}' not found.") };
            }

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var member in members)
            {
                sb.AppendLine($"Member: {member.Name}, Sum: {member.Sum}");
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }
    }
}
