using Flobot.Common;
using Flobot.Settings;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public interface IExternalSource
    {
        string GetReplyMessage(ActivityBundle bundle);

        void SetExternalConnectionData(ExternalConnectionDataDTO data);

        bool Connect();
    }
}
