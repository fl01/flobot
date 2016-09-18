﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Logging;
using Flobot.Messages.Handlers;
using Flobot.Messages.Handlers.ExternalHandler;
using Flobot.Settings;

namespace Flobot.Messages
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        private ActivityBundle activityBundle;
        private readonly ILog logger;
        private ISettingsService settingsService;

        public MessageHandlerProvider(ActivityBundle activityBundle)
        {
            this.activityBundle = activityBundle;
            this.logger = IoC.Container.Resolve<ILoggingService>().GetLogger(this);
            this.settingsService = IoC.Container.Resolve<ISettingsService>();
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

                return CreateHandler(matchedHandlerType, activityBundle);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        private IMessageHandler CreateHandler(Type type, ActivityBundle bundle)
        {
            if (typeof(ExternalHandlerBase).IsAssignableFrom(type))
            {
                IExternalSource source = IoC.Container.Resolve<IExternalSource>(nameof(RestfulSource));
                return Activator.CreateInstance(type, source, activityBundle) as IMessageHandler;
            }
            else
            {
                return Activator.CreateInstance(type, activityBundle) as IMessageHandler;
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
