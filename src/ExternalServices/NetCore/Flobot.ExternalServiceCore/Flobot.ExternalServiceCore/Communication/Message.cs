﻿namespace Flobot.ExternalServiceCore.Communication
{
    public class Message
    {
        public string RawText { get; set; }

        public string Command { get; set; }

        public string SubCommand { get; set; }

        public string CommandArg { get; set; }

        public bool IsCommand { get; set; }
    }
}
