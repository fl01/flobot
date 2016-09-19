using System.Collections.Generic;
using Flobot.Common;
using Flobot.Common.ExternalServices;
using Flobot.Settings;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public interface IExternalSource
    {
        void SetExternalConnectionData(ExternalConnectionDataDTO data);

        bool Connect();

        IEnumerable<ExternalReply> GetReplyMessages(ActivityBundle bundle);
    }
}
