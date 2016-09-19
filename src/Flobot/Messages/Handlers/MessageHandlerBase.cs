using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Logging;
using Flobot.Messages.Commands;
using Flobot.Settings;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        protected const string UnknownSubCommandError = "Unknown subcommand. Try use /? to see a list of available commands";

        protected ActivityBundle ActivityBundle { get; private set; }

        protected ILog Logger { get; private set; }

        protected Dictionary<ICommandInfo, Func<IEnumerable<Activity>>> SubCommands { get; set; }

        protected ISettingsService SettingsService { get; private set; }

        public MessageHandlerBase(ActivityBundle activityBundle)
        {
            ActivityBundle = activityBundle;
            Logger = IoC.Container.Resolve<ILoggingService>().GetLogger(this);
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>();
            SettingsService = IoC.Container.Resolve<ISettingsService>();
        }

        public IEnumerable<Activity> GetReplies()
        {
            try
            {
                if ("/?".Equals(ActivityBundle.Message.CommandArg, StringComparison.CurrentCultureIgnoreCase))
                {
                    return CreateHelpReplies();
                }
                else
                {
                    return CreateReplies();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return CreateSingleReplyCollection("Internal error");
            }
        }

        protected ThumbnailCard CreateThumbnailCard()
        {
            return CreateThumbnailCard(string.Empty);
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

        protected abstract IEnumerable<Activity> CreateReplies();

        protected virtual IEnumerable<Activity> CreateHelpReplies()
        {
            var permittedSubCommands = GetPermittedSubCommands();

            if (!permittedSubCommands.Any())
            {
                return CreateSingleReplyCollection("There are no subcommands available");
            }

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);
            sb.AppendLine("List of subcommands:");

            foreach (var keyValuePair in permittedSubCommands)
            {
                sb.AppendLine($"{SettingsService.GetSubCommandSeparator()}{keyValuePair.Key.Name}");
            }

            return CreateSingleReplyCollection(sb.ToString());
        }

        protected IEnumerable<KeyValuePair<ICommandInfo, Func<IEnumerable<Activity>>>> GetPermittedSubCommands()
        {
            return SubCommands.Where(x => x.Key.CanExecute(ActivityBundle));
        }

        protected KeyValuePair<ICommandInfo, Func<IEnumerable<Activity>>> GetPermittedSubCommand(string subCommand)
        {
            return GetPermittedSubCommands().FirstOrDefault(c => c.Key.Name.Equals(subCommand, StringComparison.CurrentCultureIgnoreCase));
        }

        protected virtual IEnumerable<Activity> GetInvalidSubCommandReply()
        {
            return CreateSingleReplyCollection(UnknownSubCommandError);
        }

        protected virtual IEnumerable<Activity> CreateSingleReplyCollection(string text)
        {
            return new[] { CreateSingleReply(text) };
        }

        protected virtual Activity CreateSingleReply(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return ActivityBundle.Activity.CreateReply(text);
        }

        protected Activity CreateThumbnailCardReply(ThumbnailCard card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            Activity imageReply = CreateSingleReply(string.Empty);
            imageReply.AttachmentLayout = "carousel";
            imageReply.Attachments.Add(card.ToAttachment());

            return imageReply;
        }
    }
}
