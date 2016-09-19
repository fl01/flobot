using System;
using System.Collections.Generic;
using Flobot.Common;
using Flobot.Common.ExternalServices;
using Flobot.Identity;
using Flobot.Messages.Handlers.ExternalHandler;
using Flobot.Settings;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("go lang", Section.Default, "golang", "gl")]
    public class GolangHandler : ExternalHandlerBase
    {   
        public GolangHandler(IExternalSource source, ActivityBundle activityBundle)
            : base(source, activityBundle)
        {
            ExternalConnectionDataDTO connectionData = SettingsService.GetGolangConnectionData();
            source.SetExternalConnectionData(connectionData);
        }
    }
}
