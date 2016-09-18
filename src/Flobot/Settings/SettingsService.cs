using System;
using Flobot.Logging;

namespace Flobot.Settings
{
    public class SettingsService : ISettingsService
    {
        private IConfigSettings configSettings;
        private readonly ILog logger;

        public SettingsService(ILoggingService loggingService, IConfigSettings configSettings)
        {
            this.logger = loggingService.GetLogger(this);
            this.configSettings = configSettings;
        }

        public string GetCommandPrefix()
        {
            return configSettings.GetCommandPrefix();
        }

        public ExternalConnectionDataDTO GetGolangConnectionData()
        {
            return new ExternalConnectionDataDTO()
            {
                Url = new Uri(configSettings.GetGolangExternalHandlerHost())
            };
        }

        public string GetSubCommandSeparator()
        {
            return configSettings.GetSubCommandSeparator();
        }
    }
}