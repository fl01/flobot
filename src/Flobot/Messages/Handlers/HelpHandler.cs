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
    [Message("get your help today", Section.Help, "help")]
    public class HelpHandler : MessageHandlerBase
    {
        public HelpHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            var replyString = GetReplyMessage();
            return CreateSingleReplyCollection(replyString);
        }

        private string GetReplyMessage()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            sb.AppendLine($"{ActivityBundle.Caller.Name}, list of your commands is:");
            sb.AppendLine();

            IEnumerable<Type> permittedHandlers = GetPermittedHandlers();

            var sortedHandlers = permittedHandlers.Select(x => new { Handler = x, Attribute = x.GetCustomAttribute<MessageAttribute>() });

            foreach (var data in sortedHandlers.OrderBy(x => x.Attribute?.Section).ThenBy(x => x.Attribute?.SupportedCommands?.FirstOrDefault()))
            {
                var messageAttribute = data.Handler.GetCustomAttribute<MessageAttribute>();

                if (messageAttribute == null || messageAttribute.SupportedCommands == null || !messageAttribute.SupportedCommands.Any())
                {
                    Logger.Warn($"Handler {data.Handler.Name} has no defined supported commands");
                    continue;
                }

                string commandPrefix = SettingsService.GetCommandPrefix();
                string formattedCommands = string.Join(", ", messageAttribute.SupportedCommands.Select(commnad => commandPrefix + commnad));
                string fullCommandInfo = formattedCommands + " - " + messageAttribute.Description;
                sb.AppendLine(fullCommandInfo);
            }

            if (permittedHandlers.Any())
            {
                sb.AppendLine();
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
