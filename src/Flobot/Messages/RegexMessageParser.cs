using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Flobot.Messages
{
    public class RegexMessageParser : IMessageParser
    {
        private const string CommandPrefix = "!";
        private const string SubCommandDelimeter = ".";

        private readonly string commandPattern = $"^{CommandPrefix}([a-zA-Z0-9]+)([{SubCommandDelimeter}]([a-zA-Z0-9]+))?"; // represents !command.subcommand

        public Message Parse(string text)
        {
            Message message = new Message(text.Trim());

            var commandParseResult = ParseCommand(message.RawText);

            message.Command = commandParseResult.Command;
            message.CommandArg = commandParseResult.CommandArg;
            message.SubCommand = commandParseResult.SubCommand;

            return message;
        }

        private CommandParseResult ParseCommand(string text)
        {
            try
            {
                Match commandMatch = Regex.Match(text, commandPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(30));

                if (!commandMatch.Success)
                {
                    return CommandParseResult.Empty;
                }

                CommandParseResult parseResult = new CommandParseResult();

                parseResult.Command = commandMatch.Groups[1].Value;
                parseResult.SubCommand = commandMatch.Groups[3].Value;
                parseResult.CommandArg = text.Substring(commandMatch.Index + commandMatch.Length).Trim();

                return parseResult;
            }
            catch (Exception)
            {
                // TODO : log
                return CommandParseResult.Empty;
            }
        }
    }
}
