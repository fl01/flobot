using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace Flobot.Identity
{
    public class UserManager : IUserManager
    {
        private IUserStore userStore;

        public UserManager(IUserStore userStore)
        {
            this.userStore = userStore;
        }

        public IEnumerable<User> GetUsers()
        {
            return userStore.GetUsers();
        }

        public User RecognizeUser(ChannelAccount account)
        {
            var registeredUser = userStore.GetUsers().FirstOrDefault(u => u.Id.Equals(account.Id));

            if (registeredUser != null)
            {
                return registeredUser;
            }

            // user is not registered in the system
            return new User()
            {
                Role = Role.User,
                Id = account.Id,
                Name = account.Name,
                Group = Group.Default
            };
        }
    }
}
