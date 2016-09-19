using System;

namespace Flobot.Common.DTO
{
    public class ExternalConnectionDataDTO
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public Uri Url { get; set; }
    }
}
