using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Identity;
using Flobot.Messages;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Flobot.Common
{
    public class ActivityBundle
    {
        [JsonIgnore]
        public Activity Activity { get; private set; }

        public Message Message { get; private set; }

        public User Caller { get; private set; }

        public ActivityBundle(Activity activity, Message message, User caller)
        {
            Activity = activity;
            Message = message;
            Caller = caller;
        }
    }
}