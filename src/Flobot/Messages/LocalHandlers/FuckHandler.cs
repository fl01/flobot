using System.Collections.Generic;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Handlers;
using Flobot.Messages.LocalHandlers.Fuck;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("modern solution to the common problem of telling people to fuck off", Section.Default, "fuck", "fk")]
    public class FuckHandler : MessageHandlerBase
    {
        private FoaasProxy proxy;

        private FoaasProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = IoC.Container.Resolve<FoaasProxy>();
                }

                return proxy;
            }
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            string replyMessage = GetReplyMessage(activityBundle);
            return CreateSingleReplyCollection(activityBundle, replyMessage);
        }

        private string GetReplyMessage(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                return Proxy.GetRandomFromReply(activityBundle.Caller.Name);
            }
            else
            {
                return Proxy.GetRandomFromToNameReply(activityBundle.Message.CommandArg, activityBundle.Caller.Name);
            }
        }
    }
}
