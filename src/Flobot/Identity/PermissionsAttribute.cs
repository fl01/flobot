using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PermissionsAttribute : Attribute
    {
        public Role Role { get; private set; }

        public Group Group { get; private set; }

        // This is a positional argument
        public PermissionsAttribute(Role role, Group group = Group.Default)
        {
            Role = role;
            Group = group;
        }
    }
}
