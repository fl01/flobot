using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Flobot.Settings
{
    public class ExternalConnectionDataDTO
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public Uri Url { get; set; }
    }
}