using Flobot.Common;
using Flobot.Messages.Handlers;

namespace Flobot.Messages
{
    public interface IMessageHandlerProvider
    {
        IMessageHandler GetHandler(ActivityBundle activityBundle);
    }
}
