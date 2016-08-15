using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message(Section.Default, "account", "acc")]
    public class AccountHandler : MessageHandlerBase
    {
        private IUserManager userManager;

        public AccountHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            userManager = IoC.Container.Resolve<IUserManager>();
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                Logger.Debug("Subcommand is empty");
                return CreateHelpReplies();
            }

            var subCommand = GetPermittedSubCommand(ActivityBundle.Message.SubCommand);
            if (subCommand.Value == null)
            {
                return new[] { ActivityBundle.Activity.CreateReply($"Invalid subcommand '{ActivityBundle.Message.SubCommand}'") };
            }

            return subCommand.Value();
        }

        private IEnumerable<Activity> GetAllUsers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            var users = userManager.GetUsers();

            foreach (var user in users.OrderBy(u => u.Role))
            {
                sb.AppendLine($"ID='{user.Id}', Name='{user.Name}', Role='{user.Role}', Group='{user.Group}'");
            }

            sb.AppendLine($"Total users: {users.Count()}");

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("users", Group.Administrators, Role.Admin), GetAllUsers}
            };
        }
    }
}