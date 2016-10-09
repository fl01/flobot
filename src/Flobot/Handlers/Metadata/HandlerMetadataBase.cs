using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Flobot.Messages;
using Flobot.Messages.Commands;

namespace Flobot.Handlers.Metadata
{
    public abstract class HandlerMetadataBase : IHandlerMetadata
    {
        public abstract HandlerType HandlerType { get; }

        public string Description { get; set; }

        public Group Group { get; set; }

        public Guid Id { get; set; }

        public Role Role { get; set; }

        public IReadOnlyCollection<ICommandInfo> SubCommands { get; set; }

        public IReadOnlyCollection<string> SupportedCommands { get; set; }

        public Section Section { get; set; }
    }
}