using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("manage your account", Section.Default, "account", "acc")]
    public class AccountHandler : MessageHandlerBase
    {
        private IUserManager userManager;

        public AccountHandler()
        {
            userManager = IoC.Container.Resolve<IUserManager>();
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                Logger.Debug("Subcommand is empty");
                return CreateHelpReplies(activityBundle);
            }

            var subCommand = GetPermittedSubCommand(activityBundle, activityBundle.Message.SubCommand);
            if (subCommand.Value == null)
            {
                return GetInvalidSubCommandReply(activityBundle);
            }

            return subCommand.Value(activityBundle);
        }

        private IEnumerable<Activity> GetAllUsers(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            var users = userManager.GetUsers();

            foreach (User user in users.OrderBy(u => u.Role))
            {
                sb.AppendLine($"ID='{user.Id}', Name='{user.Name}', Role='{user.Role}', Group='{user.Group}'");
            }

            sb.AppendLine($"Total users: {users.Count()}");

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }

        private void InitializeSubCommands()
        {
            SubCommands.Add(new ChatCommandInfo("users", Group.Administrators, Role.Admin), GetAllUsers);
        }
    }
}