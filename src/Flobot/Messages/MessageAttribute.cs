using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MessageAttribute : Attribute
    {
        public string Description { get; private set; }

        public IEnumerable<string> SupportedCommands { get; private set; }

        public Section Section { get; private set; }

        public MessageAttribute(string description, Section section, params string[] supportedCommands)
        {
            Description = description;
            Section = section;
            SupportedCommands = supportedCommands;
        }
    }
}
