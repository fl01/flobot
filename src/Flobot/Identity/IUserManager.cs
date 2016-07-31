using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace Flobot.Identity
{
    public interface IUserManager
    {
        User RecognizeUser(ChannelAccount account);
    }
}
