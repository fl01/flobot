using Microsoft.Extensions.Configuration;

namespace Flobot.ExternalServiceCore.Settings
{
    public abstract class ConfigSettingsBase
    {
        private IConfigurationRoot root;

        public ConfigSettingsBase(IConfigurationRoot root)
        {
            this.root = root;
        }

        public string GetKeyValue(string key)
        {
            return root[key];
        }

        public void SetKeyValue(string key, string value)
        {
            root[key] = value;
        }

        public string GetDataSourceKeyValue(string key)
        {
            return root.GetSection("DataSource")?[key] ?? string.Empty;
        }
    }
}
