using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        protected Message Message { get; private set; }

        protected User Caller { get; private set; }

        public MessageHandlerBase(User caller, Message message)
        {
            Message = message;
            Caller = caller;
        }

        public Activity CreateReply(Activity activity)
        {
            string replyMessage;

            try
            {
                replyMessage = GetReplyMessage(activity);
            }
            catch(Exception e)
            {
                replyMessage = "Internal error. " + e.Message;
            }

            return activity.CreateReply(replyMessage);
        }

        protected abstract string GetReplyMessage(Activity activity);
    }
}
