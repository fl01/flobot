namespace Flobot.ExternalServiceLocator.Settings
{
    public interface ISettingsService
    {
        string GetConnectionString();

        string GetDbName();
    }
}
