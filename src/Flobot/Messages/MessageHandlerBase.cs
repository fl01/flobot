using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Logging;
using Flobot.Messages.Commands;
using Flobot.Settings;
using Microsoft.Bot.Connector;

namespace Flobot.Messages
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        protected const string UnknownSubCommandError = "Unknown subcommand. Try use /? to see a list of available commands";

        protected ILog Logger { get; private set; }

        public Dictionary<ICommandInfo, Func<ActivityBundle, IEnumerable<Activity>>> SubCommands { get; set; }

        protected ISettingsService SettingsService { get; private set; }

        protected IPermissionsService PermissionsService { get; private set; }

        public MessageHandlerBase()
        {
            Logger = IoC.Container.Resolve<ILoggingService>().GetLogger(this);
            SubCommands = new Dictionary<ICommandInfo, Func<ActivityBundle, IEnumerable<Activity>>>();
            SettingsService = IoC.Container.Resolve<ISettingsService>();
            PermissionsService = IoC.Container.Resolve<IPermissionsService>();
        }

        public IEnumerable<Activity> GetReplies(ActivityBundle activityBundle)
        {
            try
            {
                if ("/?".Equals(activityBundle.Message.CommandArg, StringComparison.CurrentCultureIgnoreCase))
                {
                    return CreateHelpReplies(activityBundle);
                }
                else
                {
                    return CreateReplies(activityBundle);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return CreateSingleReplyCollection(activityBundle, "Internal error");
            }
        }

        protected ThumbnailCard CreateThumbnailCard(string text)
        {
            var card = new ThumbnailCard()
            {
                Images = new List<CardImage>()
            };

            if (!string.IsNullOrEmpty(text))
            {
                // current version of Skype (7.26.01) wraps thumbnail title if it is longer than 26 symbols or 2 lines
                // so, let's display text bigger if we can (ᵔ◡ᵔ)
                if (text.Length <= 26 && text.Split('\n').Length <= 2)
                {
                    card.Title = text;
                }
                else
                {
                    card.Text = text;
                }
            }

            return card;
        }

        protected abstract IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle);

        protected virtual IEnumerable<Activity> CreateHelpReplies(ActivityBundle activityBundle)
        {
            var permittedSubCommands = GetPermittedSubCommands(activityBundle);

            if (!permittedSubCommands.Any())
            {
                return CreateSingleReplyCollection(activityBundle, "There are no subcommands available");
            }

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            sb.AppendLine("List of subcommands:");

            foreach (var keyValuePair in permittedSubCommands)
            {
                sb.AppendLine($"{SettingsService.GetSubCommandSeparator()}{keyValuePair.Key.Name}");
            }

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }

        protected KeyValuePair<ICommandInfo, Func<ActivityBundle, IEnumerable<Activity>>> GetPermittedSubCommand(ActivityBundle bundle, string subCommand)
        {
            return GetPermittedSubCommands(bundle).FirstOrDefault(c => c.Key.Name.Equals(subCommand, StringComparison.CurrentCultureIgnoreCase));
        }

        protected IEnumerable<KeyValuePair<ICommandInfo, Func<ActivityBundle, IEnumerable<Activity>>>> GetPermittedSubCommands(ActivityBundle bundle)
        {
            return SubCommands.Where(x => PermissionsService.HasPermissions(bundle.Caller.Role, bundle.Caller.Group, x.Key.Role, x.Key.Group));
        }

        protected virtual IEnumerable<Activity> GetInvalidSubCommandReply(ActivityBundle bundle)
        {
            return CreateSingleReplyCollection(bundle, UnknownSubCommandError);
        }

        protected virtual IEnumerable<Activity> CreateSingleReplyCollection(ActivityBundle bundle, string text)
        {
            return new[] { CreateSingleReply(bundle, text) };
        }

        protected virtual Activity CreateSingleReply(ActivityBundle activityBundle, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return activityBundle.Activity.CreateReply(text);
        }

        protected Activity CreateThumbnailCardReply(ActivityBundle activityBundle, ThumbnailCard card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            Activity imageReply = CreateSingleReply(activityBundle, string.Empty);
            imageReply.AttachmentLayout = "carousel";
            imageReply.Attachments.Add(card.ToAttachment());

            return imageReply;
        }
    }
}
