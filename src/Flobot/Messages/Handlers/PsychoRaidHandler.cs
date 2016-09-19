using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers.PsychoRaid;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User, Group.PsychoRaid)]
    [Message("raid helper", Section.Default, "PsychoRaid", "pr")]
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

        public PsychoRaidHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("all"), GetAllMembers }
            };
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
            var commandInfo = GetPermittedSubCommand(ActivityBundle.Message.SubCommand);

            if (commandInfo.Value == null)
            {
                return GetInvalidSubCommandReply();
            }

            return commandInfo.Value();
        }

        private IEnumerable<Activity> GetAllMembers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            IEnumerable<RaidMember> allmembers = Proxy.GetAllRaidMembers();

            foreach (var member in allmembers)
            {
                sb.AppendLine($"Member: {member.Name}, Sum: {member.Sum}");
            }

            return CreateSingleReplyCollection(sb.ToString());
        }

        private IEnumerable<Activity> GetCommandReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return CreateSingleReplyCollection("Character name is required.");
            }

            IEnumerable<RaidMember> members = Proxy.GetRaidMembers(ActivityBundle.Message.CommandArg);

            if (members == null || !members.Any())
            {
                return CreateSingleReplyCollection($"Member '{ActivityBundle.Message.CommandArg}' not found.");
            }

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var member in members)
            {
                sb.AppendLine($"Member: {member.Name}, Sum: {member.Sum}");
            }

            return CreateSingleReplyCollection(sb.ToString());
        }
    }
}
