using System.Collections.Generic;
using Flobot.Common;
using Microsoft.Bot.Connector;

namespace Flobot.Messages
{
    public interface IMessageHandler
    {
        IEnumerable<Activity> GetReplies(ActivityBundle bundle);
    }
}
