using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages
{
    public class Message
    {
        public string RawText { get; private set; }

        public string Command { get; set; }

        public string SubCommand { get; set; }

        public string CommandArg { get; set; }

        public bool IsCommand
        {
            get { return !string.IsNullOrEmpty(Command); }
        }

        public Message(string rawText)
        {
            RawText = rawText;
        }
    }
}
