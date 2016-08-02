using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Flobot.Messages.Handlers.Fuck
{
    public class SimpleJsonClient : WebClient
    {
        private const string JsonContent = "application/json";

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            var httpRequest = (request as HttpWebRequest);
            httpRequest.Accept = JsonContent;
            request.ContentType = JsonContent;
            return request;
        }
    }
}
