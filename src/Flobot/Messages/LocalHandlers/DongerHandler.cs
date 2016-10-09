using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers;
using Flobot.Messages.LocalHandlers.Donger;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("(ง'̀-'́)ง raise your dongers (ง'̀-'́)ง", Section.Default, "donger", "dnr")]
    public class DongerHandler : MessageHandlerBase
    {
        private DongerStore store;

        private DongerStore Store
        {
            get
            {
                if (store == null)
                {
                    store = new DongerStore();
                }

                return store;
            }
        }

        public DongerHandler()
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                return GetRandomDonger(activityBundle);
            }

            var replyKeyValuePair = GetPermittedSubCommand(activityBundle, activityBundle.Message.SubCommand);

            if (replyKeyValuePair.Value == null)
            {
                return GetInvalidSubCommandReply(activityBundle);
            }

            return replyKeyValuePair.Value(activityBundle);
        }

        private void InitializeSubCommands()
        {
            SubCommands.Add(new ChatCommandInfo("top"), GetPopularDongers);
            SubCommands.Add(new ChatCommandInfo("all"), GetAllDongers);
        }

        private IEnumerable<Activity> GetRandomDonger(ActivityBundle activityBundle)
        {
            var allDongers = Store.GetAllDongers();

            string randomDonger = allDongers.ElementAt(new Random().Next(allDongers.Count)).Text;
            return CreateSingleReplyCollection(activityBundle, randomDonger);
        }

        private IEnumerable<Activity> GetPopularDongers(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            var popularDongers = Store.GetAllDongers().Where(d => d.IsPopular);

            foreach (var donger in popularDongers)
            {
                sb.AppendLine(donger.Text);
            }

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }

        private IEnumerable<Activity> GetAllDongers(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var donger in Store.GetAllDongers())
            {
                sb.AppendLine(donger.Text);
            }

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }
    }
}
