using Flobot.ExternalServiceCore.Settings;

namespace Flobot.AccountsService.Settings
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
            return config.GetKeyValue("DataSource:MongoDbConnectionString");
        }

        public string GetDbName()
        {
            return config.GetKeyValue("DataSource:DbName");
        }
    }
}
