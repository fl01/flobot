using System.Collections.Generic;
using Flobot.Common;
using Flobot.Common.DTO;
using Flobot.Common.ExternalServices;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public interface IExternalSource
    {
        void SetExternalConnectionData(ExternalConnectionDataDTO data);

        bool Connect();

        IEnumerable<ExternalReply> GetReplyMessages(ActivityBundle bundle);
    }
}
