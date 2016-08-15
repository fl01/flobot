using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Logging;
using Flobot.Messages;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;

namespace Flobot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private IUserManager userManager;
        private IMessageParser messageParser;
        private readonly ILog logger;

        public MessagesController()
        {
            userManager = IoC.Container.Resolve<IUserManager>();
            messageParser = IoC.Container.Resolve<IMessageParser>();
            logger = IoC.Container.Resolve<ILoggingService>().GetLogger(this);
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            IEnumerable<Activity> replies;

            try
            {
                switch (activity.Type)
                {
                    case ActivityTypes.Message:
                        replies = HandleMessageActivity(activity);
                        break;
                    case ActivityTypes.ContactRelationUpdate:
                        replies = HandleBotAddedRemovedFromContact(activity);
                        break;
                    default:
                        replies = Enumerable.Empty<Activity>();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                replies = Enumerable.Empty<Activity>();
            }

            if (replies != null)
            {
                foreach (var reply in replies)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private IEnumerable<Activity> HandleBotAddedRemovedFromContact(Activity activity)
        {
            if ("Add".Equals(activity.Action, StringComparison.CurrentCultureIgnoreCase))
            {
                return new[] { activity.CreateReply($"Nice to meet you, {activity.From.Name}. I'm a flobot. Please type !help to see my commands. Also please visit https://flobot.enterflose.net/misc/PrivacyStatement.html to find out more about my policies.") };
            }
            else
            {
                return new[] { activity.CreateReply("Sorry to see you leaving...Good luck!") };
            }
        }

        private IEnumerable<Activity> HandleMessageActivity(Activity activity)
        {
            activity.RemoveRecipientMention();
            User user = userManager.RecognizeUser(activity.From);
            Message message = messageParser.Parse(activity.Text);

            ActivityBundle bundle = new ActivityBundle(activity, message, user);
            IMessageHandlerProvider handlerProvider = new MessageHandlerProvider(bundle);
            IMessageHandler handler = handlerProvider.GetHandler();

            List<Activity> replies = new List<Activity>();

            if (handler == null)
            {
                if (message.IsCommand)
                {
                    var unknownCommandReply = activity.CreateReply("Unknown command. Please type !help to see your list of supported commands.");
                    replies.Add(unknownCommandReply);
                }
                else // should never occur
                {
                    var internalErrorReply = activity.CreateReply($"Internal error occurred. Please try again later.");
                    replies.Add(internalErrorReply);
                }
            }
            else
            {
                var successReplies = handler.GetReplies();
                replies.AddRange(successReplies);
            }

            return replies;
        }
    }
}
