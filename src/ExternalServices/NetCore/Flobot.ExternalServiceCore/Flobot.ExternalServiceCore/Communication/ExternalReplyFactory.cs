using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Communication
{
    public static class ExternalReplyFactory
    {
        public static ExternalReply[] CreateSingleTextReplyCollection(string text)
        {
            var reply = CreateEmptyReply(ReplyType.Text);
            reply.Text = text;
            return new[] { reply };
        }

        private static ExternalReply CreateEmptyReply(ReplyType kind)
        {
            return new ExternalReply() { Kind = kind };
        }
    }
}
