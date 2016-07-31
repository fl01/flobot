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
                new User() { Role = Role.Admin, Id = "29:1bg6E1DleuImOkBTeV71vPluAqvkdHJNxPidOccIXTW8", Name = "Artem Vasylenko" }
            };
        }

        public IReadOnlyCollection<User> GetUsers()
        {
            return users;
        }
    }
}
