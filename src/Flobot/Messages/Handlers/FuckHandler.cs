﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Handlers.Fuck;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
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

        public FuckHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            string replyMessage = GetReplyMessage();
            return CreateSingleReplyCollection(replyMessage);
        }

        private string GetReplyMessage()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return Proxy.GetRandomFromReply(ActivityBundle.Caller.Name);
            }
            else
            {
                return Proxy.GetRandomFromToNameReply(ActivityBundle.Message.CommandArg, ActivityBundle.Caller.Name);
            }
        }
    }
}
