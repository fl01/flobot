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
