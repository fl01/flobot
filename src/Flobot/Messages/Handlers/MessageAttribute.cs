using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class MessageAttribute : Attribute
    {
        public IEnumerable<string> SupportedCommands { get; private set; }

        public MessageAttribute(params string[] supportedCommands)
        {
            SupportedCommands = supportedCommands;
        }
    }
}
