using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public int GetElapsedHoursFromBuildDateRound()
        {
            return configSettings.GetElapsedHoursFromBuildDateRound();
        }

        public string GetSubCommandSeparator()
        {
            return configSettings.GetSubCommandSeparator();
        }
    }
}