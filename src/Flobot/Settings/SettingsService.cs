using System;
using Flobot.Common.DTO;
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

        public string GetSubCommandSeparator()
        {
            return configSettings.GetSubCommandSeparator();
        }

        public ExternalConnectionDataDTO GetTempEmailConnectionData()
        {
            return new ExternalConnectionDataDTO()
            {
                Url = new Uri(configSettings.GetTempEmailExternalHandlerHost())
            };
        }

        public TimeSpan GetUpdateHandlersFrequency()
        {
            return configSettings.GetUpdateHandlersFrequency();
        }
    }
}