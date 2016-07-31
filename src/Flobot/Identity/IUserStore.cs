using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    public interface IUserStore
    {
        IReadOnlyCollection<User> GetUsers();
    }
}
