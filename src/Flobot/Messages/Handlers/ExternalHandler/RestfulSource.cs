using System;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Settings;

namespace Flobot.Messages.Handlers.ExternalHandler
{
    public class RestfulSource : IExternalSource
    {
        private ExternalConnectionDataDTO connectionData;

        public RestfulSource(ExternalConnectionDataDTO connectionData)
        {
            this.connectionData = connectionData;
        }

        public bool Connect()
        {
            var client = IoC.Container.Resolve<HttpClient>();
            return client.Ping(connectionData.Url);
        }

        public string GetReplyMessage(ActivityBundle bundle)
        {
            var client = IoC.Container.Resolve<HttpClient>();
            return client.PostJson(connectionData.Url, bundle.Message);
        }

        public void SetExternalConnectionData(ExternalConnectionDataDTO data)
        {
            connectionData = data;
        }
    }
}
