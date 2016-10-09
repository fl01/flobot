using System;
using Flobot.Common.DTO;

namespace Flobot.Settings
{
    public interface ISettingsService
    {
        string GetCommandPrefix();

        string GetSubCommandSeparator();

        ExternalConnectionDataDTO GetTempEmailConnectionData();

        TimeSpan GetUpdateHandlersFrequency();
    }
}