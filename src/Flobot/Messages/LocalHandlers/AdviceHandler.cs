using System;
using System.Collections.Generic;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers;
using Flobot.Messages.LocalHandlers.Advice;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("get some life changing advice!", Section.Default, "advice", "adv")]
    public class AdviceHandler : MessageHandlerBase
    {
        private AdviceProvider adviceProvider;

        public AdviceHandler()
        {
            adviceProvider = new AdviceProvider();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            string replyMessage = GetReplyMessage(activityBundle);
            return CreateSingleReplyCollection(activityBundle, replyMessage);
        }

        private string GetReplyMessage(ActivityBundle activityBundle)
        {
            string advice = adviceProvider.GetAdvice();

            string fromLowerChar = TransformToLowerStart(advice);

            if (!string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                return $"{activityBundle.Message.CommandArg}, {fromLowerChar}";
            }
            else
            {
                return $"{activityBundle.Caller.Name}, {fromLowerChar}";
            }
        }

        private string TransformToLowerStart(string text)
        {
            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }
    }
}
