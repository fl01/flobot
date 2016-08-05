using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    public class UserStore : IUserStore
    {
        private List<User> users;

        public UserStore()
        {
            users = new List<User>()
            {
                new User() { Id = "29:1bg6E1DleuImOkBTeV71vPluAqvkdHJNxPidOccIXTW8", Name = "Artem V", Group = Group.Administrators, Role = Role.Admin },
                new User() { Id = "29:1ANMOkUOM9d4f8-DgoQZeq6nbIW5MbJ0-WqjwvNY5DLM", Name = "psycho", Group = Group.Default | Group.PsychoRaid, Role = Role.User }
            };
        }

        public IReadOnlyCollection<User> GetUsers()
        {
            return users;
        }
    }
}
