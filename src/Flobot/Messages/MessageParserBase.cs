using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages
{
    public abstract class MessageParserBase : IMessageParser
    {
        private const string BotName = "@flobot";

        protected abstract Message InternalParse(string text);

        public Message Parse(string rawText)
        {
            string cleanedText = RemoveBotNameIfNeeded(rawText);

            return InternalParse(cleanedText);
        }

        private string RemoveBotNameIfNeeded(string rawText)
        {
            if (!string.IsNullOrEmpty(rawText) && rawText.StartsWith(BotName))
            {
                return rawText.Substring(BotName.Length).Trim();
            }

            return rawText;
        }
    }
}
