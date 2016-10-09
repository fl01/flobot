using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Handlers;
using Flobot.Handlers.DataSource;
using Flobot.Handlers.Metadata;
using Flobot.Identity;
using Flobot.Logging;
using Flobot.Messages.Handlers;
using Flobot.Messages.LocalHandlers;
using Flobot.Settings;

namespace Flobot.Messages
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        private readonly IMetadataConverter converter;
        private readonly IHandlerMetadataService handlerService;
        private readonly ILog logger;
        private readonly IPermissionsService permissionsService;
        private readonly ISettingsService settingsService;

        public MessageHandlerProvider(
            ILoggingService loggingService,
            ISettingsService settingsService,
            IHandlerMetadataService handlerService,
            IMetadataConverter converter,
            IPermissionsService permissionsService)
        {
            this.logger = loggingService.GetLogger(this);
            this.settingsService = settingsService;
            this.handlerService = handlerService;
            this.converter = converter;
            this.permissionsService = permissionsService;
        }

        public IMessageHandler GetHandler(ActivityBundle activityBundle)
        {
            var handlersMetadata = handlerService.GetHandlersMetadata();

            if (!activityBundle.Message.IsCommand)
            {
                return new NonCommandMessageHandler();
            }

            IEnumerable<IHandlerMetadata> matchedHandlers = handlersMetadata
                // filter by permissions
                .Where(handler => permissionsService.HasPermissions(activityBundle.Caller.Role, activityBundle.Caller.Group, handler.Role, handler.Group))
                // filter by supported commands
                .Where(handler => handler.SupportedCommands.Any(c => activityBundle.Message.Command.Equals(c, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();

            if (matchedHandlers.Count() > 1)
            {
                logger.Warn("Multiple handlers are handling same commands: " + string.Join(",", matchedHandlers.Select(x => x.Id.ToString())));
            }

            // local are prioritized over any other
            IHandlerMetadata targetHandler = matchedHandlers.FirstOrDefault(x => x.HandlerType == HandlerType.Local) ?? matchedHandlers.FirstOrDefault();

            IMessageHandler messageHandler = converter.Convert(targetHandler);

            return messageHandler;
        }
    }
}
