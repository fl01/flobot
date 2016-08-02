using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public DongerHandler(User caller, Message message)
            : base(caller, message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            switch (Message.SubCommand)
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
            return string.Join(SkypeNewLine, Store.GetAllDongers().Where(d => d.IsPopular).Select(d => d.Text));
        }

        private string GetAllDongers()
        {
            return string.Join(SkypeNewLine, Store.GetAllDongers().Select(d => d.Text));
        }
    }
}
