using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Flobot.Identity;
using Flobot.Messages;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Flobot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private IUserManager userManager;
        private IMessageParser messageParser;

        public MessagesController()
        {
            userManager = new UserManager(new UserStore());
            messageParser = new RegexMessageParser();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            Activity reply;

            try
            {
                switch (activity.Type)
                {
                    case ActivityTypes.Message:
                        reply = HandleMessageActivity(activity);
                        break;
                    default:
                        reply = null;
                        break;
                }
            }
            catch(Exception)
            {
                // TODO: log error
                reply = null;
            }

            if (reply != null)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(reply);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleMessageActivity(Activity activity)
        {
            User user = userManager.RecognizeUser(activity.From);
            Message message = messageParser.Parse(activity.Text);
            IMessageHandlerProvider handlerProvider = new MessageHandlerProvider(user);
            IMessageHandler handler = handlerProvider.GetHandler(message);

            Activity reply;

            if (handler == null)
            {
                if (message.IsCommand)
                {
                    reply = activity.CreateReply("Unknown command. Please type !help to see your list of supported commands.");
                }
                else // should never occur
                {
                    reply = activity.CreateReply($"Internal error occurred. Please try again later.");
                }
            }
            else
            {
                reply = handler.CreateReply(activity);
            }

            return reply;
        }
    }
}
