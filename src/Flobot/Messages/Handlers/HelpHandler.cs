using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message(Section.Help, "help")]
    public class HelpHandler : MessageHandlerBase
    {
        public HelpHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            var replyString = GetReplyMessage();
            return new[] { ActivityBundle.Activity.CreateReply(replyString) };
        }

        private string GetReplyMessage()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            sb.AppendLine($"{ActivityBundle.Caller.Name}, list of your commands is:");

            IEnumerable<Type> permittedHandlers = GetPermittedHandlers();

            var sortedHandlers = permittedHandlers.Select(x => new { Handler = x, Attribute = x.GetCustomAttribute<MessageAttribute>() });

            foreach (var data in sortedHandlers.OrderBy(x => x.Attribute?.Section).ThenBy(x => x.Attribute?.SupportedCommands?.FirstOrDefault()))
            {
                var supportedCommands = data.Handler.GetCustomAttribute<MessageAttribute>()?.SupportedCommands;

                if (supportedCommands == null || !supportedCommands.Any())
                {
                    Logger.Warn($"Handler {data.Handler.Name} has no defined supported commands");
                    continue;
                }

                string commandPrefix = SettingsService.GetCommandPrefix();
                string formattedCommands = string.Join(", ", supportedCommands.Select(commnad => commandPrefix + commnad));
                sb.AppendLine(formattedCommands);
            }

            if (permittedHandlers.Any())
            {
                sb.AppendLine($"Also try using '{SettingsService.GetCommandPrefix()}[Command] /?' to see a list of supported subcommands");
            }

            return sb.ToString();
        }

        private IEnumerable<Type> GetPermittedHandlers()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetPermittedTypes<IMessageHandler>(ActivityBundle.Caller);
        }
    }
}
