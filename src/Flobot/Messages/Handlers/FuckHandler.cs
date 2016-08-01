using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Flobot.Messages.Handlers.Fuck;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("fuck", "fk")]
    public class FuckHandler : MessageHandlerBase
    {
        private FoaasProxy proxy;

        private FoaasProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new FoaasProxy();
                }

                return proxy;
            }

        }
        public FuckHandler(User caller, Message message)
            : base(caller, message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            if (string.IsNullOrEmpty(Message.CommandArg))
            {
                return GetFromReply();
            }
            else
            {
                return GetFromToNameReply();
            }
        }

        private string GetFromReply()
        {
            return Proxy.GetRandomFromReply(Caller.Name);
        }

        private string GetFromToNameReply()
        {
            return Proxy.GetRandomFromToNameReply(Message.CommandArg, Caller.Name);
        }
    }
}
