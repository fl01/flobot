using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("help")]
    public class HelpHandler : MessageHandlerBase
    {
        public HelpHandler(User caller, Message message)
            : base(caller, message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            var permittedHandlers = GetPermittedHandlers();

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Caller.Name}({Caller.Role}), list of your commands is:");
            sb.Append(SkypeNewLine);

            foreach (var handler in permittedHandlers)
            {
                var supportedCommands = handler.GetCustomAttribute<MessageAttribute>()?.SupportedCommands;

                if (supportedCommands == null)
                {
                    continue;
                }
                // TODO : command prefix "!" should be taken from settings service
                string formattedCommands = string.Join(", ", supportedCommands.Select(x => "!" + x));
                sb.Append(formattedCommands);
                sb.Append(SkypeNewLine);
            }

            return sb.ToString();
        }

        private IEnumerable<Type> GetPermittedHandlers()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetPermittedTypes<IMessageHandler>(Caller);
        }
    }
}
