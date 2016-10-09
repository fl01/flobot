using Flobot.Identity;

namespace Flobot.Messages.Commands
{
    public class ChatCommandInfo : ICommandInfo
    {
        public string Name { get; private set; }

        public Group Group { get; private set; }

        public Role Role { get; private set; }

        public ChatCommandInfo(string name, Group group = Group.Default, Role role = Role.User)
        {
            Name = name;
            Group = group;
            Role = role;
        }
    }
}
