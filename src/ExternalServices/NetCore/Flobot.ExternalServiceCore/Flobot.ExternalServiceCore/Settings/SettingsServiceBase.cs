using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Settings
{
    public abstract class SettingsServiceBase
    {
        private ConfigSettingsBase config;

        public SettingsServiceBase(ConfigSettingsBase config)
        {
            this.config = config;
        }
    }
}
