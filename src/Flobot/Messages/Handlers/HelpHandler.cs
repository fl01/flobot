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
    [Message("help")]
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

            var permittedHandlers = GetPermittedHandlers();
            foreach (var handler in permittedHandlers)
            {
                var supportedCommands = handler.GetCustomAttribute<MessageAttribute>()?.SupportedCommands;

                if (supportedCommands == null)
                {
                    continue;
                }
                // TODO : command prefix "!" should be taken from settings service
                string formattedCommands = string.Join(", ", supportedCommands.Select(x => "!" + x));
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
