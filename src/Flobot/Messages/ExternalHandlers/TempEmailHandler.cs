using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.DTO;
using Flobot.Identity;
using Flobot.Messages.Handlers.ExternalHandler;

namespace Flobot.Messages.Handlers
{
    //[Permissions(Role.User)]
    //[Message("get your temporary email", Section.Default, "tempemail", "te")]
    public class TempEmailHandler : ExternalHandlerBase
    {
        public TempEmailHandler(IExternalSource source)
            : base(source)
        {
            ExternalConnectionDataDTO connectionData = SettingsService.GetTempEmailConnectionData();
            source.SetExternalConnectionData(connectionData);
        }
    }
}