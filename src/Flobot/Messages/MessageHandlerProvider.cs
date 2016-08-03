using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers;

namespace Flobot.Messages
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        private ActivityBundle activityBundle;

        public MessageHandlerProvider(ActivityBundle activityBundle)
        {
            this.activityBundle = activityBundle;
        }

        public IMessageHandler GetHandler()
        {
            if (!activityBundle.Message.IsCommand)
            {
                return new NonCommandMessageHandler(activityBundle);
            }

            try
            {
                // let's search for a handler for the requested command
                IEnumerable<Type> permittedHandlers = GetPermittedMessageHandlers();

                Type matchedHandlerType = GetFirstMatchedHandler(permittedHandlers);

                if (matchedHandlerType == null)
                {
                    return null;
                }

                return Activator.CreateInstance(matchedHandlerType, activityBundle) as IMessageHandler;
            }
            catch (Exception)
            {
                return null;
                // TODO : log
            }
        }

        private Type GetFirstMatchedHandler(IEnumerable<Type> handlers)
        {
            foreach (var handler in handlers)
            {
                IEnumerable<string> supportedCommands = handler.GetCustomAttribute<MessageAttribute>()?.SupportedCommands;

                foreach (string command in supportedCommands)
                {
                    if (command.Equals(activityBundle.Message.Command, StringComparison.CurrentCultureIgnoreCase))
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
                .GetPermittedTypes<IMessageHandler>(activityBundle.Caller);
        }
    }
}
