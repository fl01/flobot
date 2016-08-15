using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

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

        public bool CanExecute(ActivityBundle bundle)
        {
            return Role <= bundle.Caller.Role && bundle.Caller.Group.HasFlag(bundle.Caller.Group);
        }
    }
}
