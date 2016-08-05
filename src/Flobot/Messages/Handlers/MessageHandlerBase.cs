using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Logging;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        protected ActivityBundle ActivityBundle { get; private set; }

        protected ILog Logger { get; private set; }

        public MessageHandlerBase(ActivityBundle activityBundle)
        {
            ActivityBundle = activityBundle;
            Logger = this.GetLogger();
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
                return new[] { ActivityBundle.Activity.CreateReply("Internal error") };
            }
        }

        protected ThumbnailCard CreateThumbnailCard()
        {
            return new ThumbnailCard()
            {
                Images = new List<CardImage>()
            };

        }

        protected abstract IEnumerable<Activity> CreateReplies();

        protected abstract IEnumerable<Activity> CreateHelpReplies();
    }
}
