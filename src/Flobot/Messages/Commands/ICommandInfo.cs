using Flobot.Identity;

namespace Flobot.Messages.Commands
{
    public interface ICommandInfo
    {
        string Name { get; }

        Role Role { get; }

        Group Group { get; }
    }
}
