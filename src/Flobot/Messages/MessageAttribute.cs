using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MessageAttribute : Attribute
    {
        public IEnumerable<string> SupportedCommands { get; private set; }

        public Section Section { get; private set; }

        public MessageAttribute(Section section, params string[] supportedCommands)
        {
            Section = section;
            SupportedCommands = supportedCommands;
        }
    }
}
