namespace Flobot.Settings
{
    public interface IConfigSettings
    {
        string GetCommandPrefix();

        string GetSubCommandSeparator();

        string GetGolangExternalHandlerHost();
    }
}