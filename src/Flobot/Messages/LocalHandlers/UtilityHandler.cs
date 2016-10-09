using System;
using System.Collections.Generic;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("some useful stuff here", Section.Default, "utility", "util")]
    public class UtilityHandler : MessageHandlerBase
    {
        public UtilityHandler()
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                Logger.Debug("Subcommand is empty");
                return CreateHelpReplies(activityBundle);
            }

            var commandInfo = GetPermittedSubCommand(activityBundle, activityBundle.Message.SubCommand);
            if (commandInfo.Value == null)
            {
                return GetInvalidSubCommandReply(activityBundle);
            }

            return commandInfo.Value(activityBundle);
        }

        private void InitializeSubCommands()
        {
            SubCommands.Add(new ChatCommandInfo("upper"), GetUppercasedText);
            SubCommands.Add(new ChatCommandInfo("lower"), GetLowercasedText);
            SubCommands.Add(new ChatCommandInfo("guid"), GetGuid);
            SubCommands.Add(new ChatCommandInfo("base64"), GetBase64);
            SubCommands.Add(new ChatCommandInfo("base64dec"), GetDecodedBase64);
            SubCommands.Add(new ChatCommandInfo("rand"), GetRandomNumber);
        }

        private IEnumerable<Activity> GetUppercasedText(ActivityBundle activityBundle)
        {
            string reply;

            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                reply = "Text is required";
            }
            else
            {
                reply = activityBundle.Message.CommandArg.ToUpper();
            }

            return CreateSingleReplyCollection(activityBundle, reply);
        }

        private IEnumerable<Activity> GetLowercasedText(ActivityBundle activityBundle)
        {
            string reply;

            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                reply = "Text is required";
            }
            else
            {
                reply = activityBundle.Message.CommandArg.ToLower();
            }

            return CreateSingleReplyCollection(activityBundle, reply);
        }

        private IEnumerable<Activity> GetBase64(ActivityBundle activityBundle)
        {
            string reply;

            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                reply = "Parameter is required";
            }
            else
            {
                reply = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(activityBundle.Message.CommandArg));
            }

            return CreateSingleReplyCollection(activityBundle, reply);
        }

        private IEnumerable<Activity> GetDecodedBase64(ActivityBundle activityBundle)
        {
            string reply;

            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                reply = "Parameter is required";
            }
            else
            {
                try
                {
                    reply = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(activityBundle.Message.CommandArg));
                }
                catch (FormatException ex)
                {
                    Logger.Error(ex);
                    reply = "Invalid base64 string";
                }
            }

            return CreateSingleReplyCollection(activityBundle, reply);
        }

        private IEnumerable<Activity> GetGuid(ActivityBundle activityBundle)
        {
            return CreateSingleReplyCollection(activityBundle, Guid.NewGuid().ToString("D"));
        }

        private IEnumerable<Activity> GetRandomNumber(ActivityBundle activityBundle)
        {
            int maxValue = 0;

            if (!string.IsNullOrEmpty(activityBundle.Message.CommandArg) && !int.TryParse(activityBundle.Message.CommandArg, out maxValue))
            {
                return CreateSingleReplyCollection(activityBundle, $"{activityBundle.Message.CommandArg} is not a valid number");
            }

            int randomValue;
            if (maxValue > 0)
            {
                randomValue = new Random().Next(++maxValue);
            }
            else
            {
                randomValue = new Random().Next();
            }

            return CreateSingleReplyCollection(activityBundle, randomValue.ToString());
        }
    }
}