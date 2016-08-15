using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Flobot.Logging;

namespace Flobot.Settings
{
    public class ConfigSettings : IConfigSettings
    {
        private readonly ILog logger;

        public ConfigSettings(ILoggingService loggingService)
        {
            this.logger = loggingService.GetLogger(this);
        }

        public string GetCommandPrefix()
        {
            return GetAppKeyValue("CommandPrefix");
        }

        public int GetElapsedHoursFromBuildDateRound()
        {
            string rawValue = GetAppKeyValue("ElapsedHoursFromBuildDateRound");

            int round;
            if (string.IsNullOrEmpty(rawValue) || !int.TryParse(rawValue, out round))
            {
                logger.Warn($"Invalid value {rawValue} cannot be parsed to int. Default value will be used instead.");
                round = 4;
            }

            return round;
        }

        public string GetSubCommandSeparator()
        {
            return GetAppKeyValue("SubCommandSeparator");
        }

        private string GetAppKeyValue(string appkey)
        {
            try
            {
                return ConfigurationManager.AppSettings[appkey];
            }
            catch (ConfigurationErrorsException ex)
            {
                logger.Error(ex);
                return string.Empty;
            }
        }
    }
}