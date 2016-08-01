using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Flobot.Identity;
using Flobot.Messages.Handlers;

namespace Flobot.Messages
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
                return new NonCommandMessageHandler(caller, message);
            }

            try
            {
                // let's search for a handler for the requested command
                IEnumerable<Type> permittedHandlers = GetPermittedMessageHandlers();

                Type matchedHandlerType = GetFirstMatchedHandler(permittedHandlers, message);

                if (matchedHandlerType == null)
                {
                    return null;
                }

                return Activator.CreateInstance(matchedHandlerType, caller, message) as IMessageHandler;
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

        private IEnumerable<Type> GetPermittedMessageHandlers()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetPermittedTypes<IMessageHandler>(caller);
        }
    }
}
