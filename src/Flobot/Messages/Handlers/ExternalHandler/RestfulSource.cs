using System;
using System.Collections.Generic;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Common.ExternalServices;
using Flobot.Settings;
using Newtonsoft.Json;

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

        public IEnumerable<ExternalReply> GetReplyMessages(ActivityBundle bundle)
        {
            var client = IoC.Container.Resolve<HttpClient>();
            string reply = client.PostJson(connectionData.Url, bundle.Message);
            return JsonConvert.DeserializeObject<ExternalReply[]>(reply);
        }

        public void SetExternalConnectionData(ExternalConnectionDataDTO data)
        {
            connectionData = data;
        }
    }
}
