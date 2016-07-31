using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Flobot.Identity;

namespace Flobot.Messages.Handlers
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        private User caller;

        public MessageHandlerProvider(User caller)
        {
            this.caller = caller;
        }

        public IMessageHandler GetHandler(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!message.IsCommand)
            {
                return GetDefaultMessageHandler(message);
            }

            try
            {
                // let's search for a handler for the requested command
                IEnumerable<Type> allMessageHandlers = GetMessageHandlers();

                IEnumerable<Type> permittedHandlers = allMessageHandlers
                    .Where(h => h.GetCustomAttribute<PermissionsAttribute>()?.Role <= caller.Role);

                Type matchedHandlerType = GetFirstMatchedHandler(permittedHandlers, message);

                if (matchedHandlerType == null)
                {
                    return null;
                }

                return Activator.CreateInstance(matchedHandlerType, message) as IMessageHandler;
            }
            catch (Exception)
            {
                return null;
                // TODO : log
            }
        }

        private Type GetFirstMatchedHandler(IEnumerable<Type> handlers, Message message)
        {
            foreach (var handler in handlers)
            {
                IEnumerable<string> supportedCommands = handler.GetCustomAttribute<MessageAttribute>()?.SupportedCommands;

                foreach (string command in supportedCommands)
                {
                    if (command.Equals(message.Command, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return handler;
                    }
                }
            }

            return null;
        }

        private IMessageHandler GetDefaultMessageHandler(Message message)
        {
            return new NonCommandMessageHandler(message);
        }

        private IEnumerable<Type> GetMessageHandlers()
        {
            Type messageHandler = typeof(IMessageHandler);
            return Assembly
                .GetExecutingAssembly()
                .GetLoadableTypes()
                .Where(messageHandler.IsAssignableFrom);
        }
    }
}
