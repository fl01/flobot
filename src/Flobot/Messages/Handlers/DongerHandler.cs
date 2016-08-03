using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers.Donger;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("donger", "dnr")]
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
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            string replyMessage = GetReplyMessage();
            return new[] { ActivityBundle.Activity.CreateReply(replyMessage) };
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            return new[] { ActivityBundle.Activity.CreateReply("...") };
        }

        private string GetReplyMessage()
        {
            switch (ActivityBundle.Message.SubCommand)
            {
                case "top":
                    return GetPopularDongers();
                case "all":
                    return GetAllDongers();
                default:
                    return GetRandomDonger();

            }
        }

        private string GetRandomDonger()
        {
            var allDongers = Store.GetAllDongers();

            return allDongers.ElementAt(new Random().Next(allDongers.Count)).Text;
        }

        private string GetPopularDongers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            var popularDongers = Store.GetAllDongers().Where(d => d.IsPopular);

            foreach (var donger in popularDongers)
            {
                sb.AppendLine(donger.Text);
            }

            return sb.ToString();
        }

        private string GetAllDongers()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            foreach (var donger in Store.GetAllDongers())
            {
                sb.AppendLine(donger.Text);
            }

            return sb.ToString();
        }
    }
}
