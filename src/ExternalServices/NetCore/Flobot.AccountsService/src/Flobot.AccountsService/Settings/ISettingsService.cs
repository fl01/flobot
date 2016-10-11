namespace Flobot.AccountsService.Settings
{
    public interface ISettingsService
    {
        string GetConnectionString();

        string GetDbName();
    }
}
