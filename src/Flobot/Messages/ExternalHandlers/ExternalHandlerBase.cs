using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Common.ExternalServices;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public abstract class ExternalHandlerBase : MessageHandlerBase
    {
        protected IExternalSource Source { get; private set; }

        public ExternalHandlerBase(IExternalSource source)
        {
            this.Source = source;
        }

        protected virtual IEnumerable<ExternalReply> GetExternalReplies(ActivityBundle activityBundle)
        {
            return Source.GetReplyMessages(activityBundle);
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            try
            {
                bool connect = Source.Connect();
                if (!connect)
                {
                    Logger.Warn("Failed to accquire connection with external source");
                    return Enumerable.Empty<Activity>();
                }

                var replies = GetExternalReplies(activityBundle);
                return ExternalReplyToActivityReply(replies, activityBundle).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        protected virtual IEnumerable<Activity> ExternalReplyToActivityReply(IEnumerable<ExternalReply> externalReplies, ActivityBundle activityBundle)
        {
            foreach (ExternalReply externalReply in externalReplies)
            {
                switch (externalReply.Kind)
                {
                    case ReplyType.Text:
                        yield return CreateSingleReply(activityBundle, externalReply.Text);
                        break;
                    case ReplyType.Thumbnail:
                        var thumbnail = CreateThumbnailCard(externalReply.Text);
                        thumbnail.Images.Add(new CardImage(externalReply.ImageUrl));
                        yield return CreateThumbnailCardReply(activityBundle, thumbnail);
                        break;
                    default:
                        Logger.Warn($"Invalid external reply type detected - {externalReply.Kind}");
                        continue;
                }
            }
        }
    }
}
