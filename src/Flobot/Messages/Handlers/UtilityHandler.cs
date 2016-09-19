using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("some useful stuff here", Section.Default, "utility", "util")]
    public class UtilityHandler : MessageHandlerBase
    {
        public UtilityHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                Logger.Debug("Subcommand is empty");
                return CreateHelpReplies();
            }

            var commandInfo = GetPermittedSubCommand(ActivityBundle.Message.SubCommand);
            if (commandInfo.Value == null)
            {
                return GetInvalidSubCommandReply();
            }

            return commandInfo.Value();
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("upper"), GetUppercasedText },
                { new ChatCommandInfo("lower"), GetLowercasedText },
                { new ChatCommandInfo("guid"), GetGuid },
                { new ChatCommandInfo("base64"), GetBase64 },
                { new ChatCommandInfo("base64dec"), GetDecodedBase64 },
                { new ChatCommandInfo("rand"), GetRandomNumber }
            };
        }

        private IEnumerable<Activity> GetUppercasedText()
        {
            string reply;

            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                reply = "Text is required";
            }
            else
            {
                reply = ActivityBundle.Message.CommandArg.ToUpper();
            }

            return CreateSingleReplyCollection(reply);
        }

        private IEnumerable<Activity> GetLowercasedText()
        {
            string reply;

            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                reply = "Text is required";
            }
            else
            {
                reply = ActivityBundle.Message.CommandArg.ToLower();
            }

            return CreateSingleReplyCollection(reply);
        }

        private IEnumerable<Activity> GetBase64()
        {
            string reply;

            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                reply = "Parameter is required";
            }
            else
            {
                reply = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ActivityBundle.Message.CommandArg));
            }

            return CreateSingleReplyCollection(reply);
        }

        private IEnumerable<Activity> GetDecodedBase64()
        {
            string reply;

            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                reply = "Parameter is required";
            }
            else
            {
                try
                {
                    reply = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ActivityBundle.Message.CommandArg));
                }
                catch (FormatException ex)
                {
                    Logger.Error(ex);
                    reply = "Invalid base64 string";
                }
            }

            return CreateSingleReplyCollection(reply);
        }

        private IEnumerable<Activity> GetGuid()
        {
            return CreateSingleReplyCollection(Guid.NewGuid().ToString("D"));
        }

        private IEnumerable<Activity> GetRandomNumber()
        {
            int maxValue = 0;

            if (!string.IsNullOrEmpty(ActivityBundle.Message.CommandArg) && !int.TryParse(ActivityBundle.Message.CommandArg, out maxValue))
            {
                return CreateSingleReplyCollection($"{ActivityBundle.Message.CommandArg} is not a valid number");
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

            return CreateSingleReplyCollection(randomValue.ToString());
        }
    }
}