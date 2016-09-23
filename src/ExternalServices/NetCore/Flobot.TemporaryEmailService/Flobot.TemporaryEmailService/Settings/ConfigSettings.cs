using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Settings;
using Microsoft.Extensions.Configuration;

namespace Flobot.TemporaryEmailService.Settings
{
    public class ConfigSettings : ConfigSettingsBase
    {
        public ConfigSettings(IConfigurationRoot root)
            : base(root)
        {
        }
    }
}
