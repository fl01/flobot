using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers.Advice;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("advice", "adv")]
    public class AdviceHandler : MessageHandlerBase
    {
        private AdviceProvider adviceProvider;

        public AdviceHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            adviceProvider = new AdviceProvider();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            string replyMessage = GetReplyMessage();
            return new[] { ActivityBundle.Activity.CreateReply(replyMessage) };
        }

        private string GetReplyMessage()
        {
            string advice = adviceProvider.GetAdvice();

            string fromLowerChar = TransformToLowerStart(advice);

            if (!string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return $"{ActivityBundle.Message.CommandArg}, {fromLowerChar}";
            }
            else
            {
                return $"{ActivityBundle.Caller.Name}, {fromLowerChar}";
            }
        }

        private string TransformToLowerStart(string text)
        {
            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }
    }
}
