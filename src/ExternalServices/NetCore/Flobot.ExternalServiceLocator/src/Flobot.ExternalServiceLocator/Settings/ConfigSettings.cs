using Flobot.ExternalServiceCore.Settings;
using Microsoft.Extensions.Configuration;

namespace Flobot.ExternalServiceLocator.Settings
{
    public class ConfigSettings : ConfigSettingsBase
    {
        public ConfigSettings(IConfigurationRoot root)
            : base(root)
        {
        }
    }
}
