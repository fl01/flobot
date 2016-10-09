namespace Flobot.Messages.Commands
{
    public class CommandParseResult
    {
        public string Command { get; set; } = string.Empty;

        public string SubCommand { get; set; } = string.Empty;

        public string CommandArg { get; set; } = string.Empty;

        public static CommandParseResult Empty
        {
            get { return new CommandParseResult(); }
        }
    }
}
