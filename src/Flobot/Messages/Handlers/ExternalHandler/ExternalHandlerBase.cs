using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public abstract class ExternalHandlerBase : MessageHandlerBase
    {
        protected IExternalSource Source { get; private set; }

        public ExternalHandlerBase(IExternalSource source, ActivityBundle activityBundle)
            : base(activityBundle)
        {
            this.Source = source;
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            try
            {
                bool connect = Source.Connect();
                if (!connect)
                {
                    Logger.Warn("Failed to accquire connection with external source");
                    return Enumerable.Empty<Activity>();
                }

                return GetExternalReply();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        protected abstract IEnumerable<Activity> GetExternalReply();
    }
}
