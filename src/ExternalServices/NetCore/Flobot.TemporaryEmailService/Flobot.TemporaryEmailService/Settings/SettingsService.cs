using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Settings;

namespace Flobot.TemporaryEmailService.Settings
{
    public class SettingsService : SettingsServiceBase, ISettingsService
    {
        private ConfigSettings config;

        public SettingsService(ConfigSettings config)
            : base(config)
        {
            this.config = config;
        }

        public string GetConnectionString()
        {
            return config.GetKeyValue("DataSource:ConnectionString");
        }
    }
}
