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
    [Message(Section.Default, "utility", "util")]
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
                return new[] { ActivityBundle.Activity.CreateReply(UnknownSubCommandError) };
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
                { new ChatCommandInfo("base64dec"), GetDecodedBase64 }
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

            return new[] { ActivityBundle.Activity.CreateReply(reply) };
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

            return new[] { ActivityBundle.Activity.CreateReply(reply) };
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

            return new[] { ActivityBundle.Activity.CreateReply(reply) };
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

            return new[] { ActivityBundle.Activity.CreateReply(reply) };
        }

        private IEnumerable<Activity> GetGuid()
        {
            return new[] { ActivityBundle.Activity.CreateReply(Guid.NewGuid().ToString("D")) };
        }
    }
}