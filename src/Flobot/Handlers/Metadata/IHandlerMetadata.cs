using System;
using System.Collections.Generic;
using Flobot.Identity;
using Flobot.Messages;
using Flobot.Messages.Commands;

namespace Flobot.Handlers.Metadata
{
    public interface IHandlerMetadata
    {
        string Description { get; }

        Section Section { get; }

        IReadOnlyCollection<string> SupportedCommands { get; }

        IReadOnlyCollection<ICommandInfo> SubCommands { get; }

        HandlerType HandlerType { get; }

        Role Role { get; }

        Group Group { get; }

        Guid Id { get; }
    }
}