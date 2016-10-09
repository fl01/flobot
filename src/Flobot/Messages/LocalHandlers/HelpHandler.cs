using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Handlers;
using Flobot.Identity;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("get your help today", Section.Help, "help")]
    public class HelpHandler : MessageHandlerBase
    {
        private readonly IHandlerMetadataService handlersService;

        public HelpHandler()
        {
            this.handlersService = IoC.Container.Resolve<IHandlerMetadataService>();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            var replyString = GetReplyMessage(activityBundle);
            return CreateSingleReplyCollection(activityBundle, replyString);
        }

        private string GetReplyMessage(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            sb.AppendLine($"{activityBundle.Caller.Name}, list of your commands is:");
            sb.AppendLine();

            var handlersMetadata = handlersService
                                    .GetHandlersMetadata()
                                    .Where(handler => PermissionsService.HasPermissions(activityBundle.Caller.Role, activityBundle.Caller.Group, handler.Role, handler.Group));

            foreach (var metadata in handlersMetadata.OrderBy(x => x.Section).ThenBy(x => x.SupportedCommands?.FirstOrDefault()))
            {
                if (metadata.SupportedCommands == null || !metadata.SupportedCommands.Any())
                {
                    Logger.Warn($"Handler {metadata.Id} has no defined supported commands");
                    continue;
                }

                string commandPrefix = SettingsService.GetCommandPrefix();
                string formattedCommands = string.Join(", ", metadata.SupportedCommands.Select(commnad => commandPrefix + commnad));
                string fullCommandInfo = formattedCommands + " - " + metadata.Description;
                sb.AppendLine(fullCommandInfo);
            }

            if (handlersMetadata.Any())
            {
                sb.AppendLine();
                sb.AppendLine($"Also try using '{SettingsService.GetCommandPrefix()}[Command] /?' to see a list of supported subcommands");
            }

            return sb.ToString();
        }
    }
}
