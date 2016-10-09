using System;

namespace Flobot.Settings
{
    public interface IConfigSettings
    {
        string GetCommandPrefix();

        string GetSubCommandSeparator();

        string GetTempEmailExternalHandlerHost();

        TimeSpan GetUpdateHandlersFrequency();
    }
}