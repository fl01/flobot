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

        public string GetSubCommandSeparator()
        {
            return GetAppKeyValue("SubCommandSeparator");
        }

        public string GetGolangExternalHandlerHost()
        {
            return GetAppKeyValue("GolangExternalHandlerHost");
        }

        public string GetTempEmailExternalHandlerHost()
        {
            return GetAppKeyValue("TempEmailExternalHandlerHost");
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