using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Commands
{
    public interface ICommandInfo
    {
        string Name { get; }

        Role Role { get; }

        Group Group { get; }

        bool CanExecute(ActivityBundle bundle);
    }
}
