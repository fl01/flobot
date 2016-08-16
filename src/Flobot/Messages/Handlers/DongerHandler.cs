using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers.Donger;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
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

        public DongerHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                return GetRandomDonger();
            }

            var replyKeyValuePair = GetPermittedSubCommands().FirstOrDefault(x => x.Key.Name.Equals(ActivityBundle.Message.SubCommand, StringComparison.CurrentCultureIgnoreCase));

            if (replyKeyValuePair.Value == null)
            {
                return new[] { ActivityBundle.Activity.CreateReply(UnknownSubCommandError) };
            }

            return replyKeyValuePair.Value();
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("top"),  GetPopularDongers },
                { new ChatCommandInfo("all"),  GetAllDongers }
            };
        }

        private IEnumerable<Activity> GetRandomDonger()
        {
            var allDongers = Store.GetAllDongers();

            return new[] { ActivityBundle.Activity.CreateReply(allDongers.ElementAt(new Random().Next(allDongers.Count)).Text) };
        }

        private IEnumerable<Activity> GetPopularDongers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            var popularDongers = Store.GetAllDongers().Where(d => d.IsPopular);

            foreach (var donger in popularDongers)
            {
                sb.AppendLine(donger.Text);
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        private IEnumerable<Activity> GetAllDongers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var donger in Store.GetAllDongers())
            {
                sb.AppendLine(donger.Text);
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }
    }
}
