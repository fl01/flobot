using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Flobot.Common
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

        public T GetJsonObject<T>(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            return GetJsonObject<T>(new Uri(address));
        }

        public T GetJsonObject<T>(Uri url)
        {
            string rawJson = DownloadString(url);
            return JsonConvert.DeserializeObject<T>(rawJson);
        }
    }
}
