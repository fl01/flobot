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

        // This is a positional argument
        public PermissionsAttribute(Role role)
        {
            Role = role;
        }
    }
}
