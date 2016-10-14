using Flobot.ExternalServiceCore.Storage;
using Flobot.ExternalServiceLocator.Settings;

namespace Flobot.ExternalServiceLocator.Storage
{
    public class MongoDbStorage : MongoDbStorageBase, IServicesStorage
    {
        private const string SettingsCollectionName = "{DBB78E78-F802-4692-A22E-F68775553D32}";

        private ISettingsService settingsService;

        public MongoDbStorage(ISettingsService settingsService)
            : base(settingsService.GetConnectionString(), settingsService.GetDbName())
        {
            this.settingsService = settingsService;
        }
    }
}
