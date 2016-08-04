using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("lirik", "lrk")]
    public class LirikHandler : MessageHandlerBase
    {
        private Dictionary<string, string> subCommands;

        private string ImageFolder
        {
            get
            {

                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Images/Lirik/";
            }
        }

        public LirikHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateHelpReplies()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            var keys = subCommands.Select(x => x.Key);

            foreach (var key in keys)
            {
                // TODO : read command/subcommand prefixes from settings service
                sb.AppendLine("!lirik." + key);
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            List<Activity> replies = new List<Activity>();

            var imageReply = ActivityBundle.Activity.CreateReply();
            imageReply.AttachmentLayout = "carousel";

            CardImage image;

            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                image = GetRandomCardImage();
            }
            else
            {
                if (!TryGetRequestedCardImage(out image))
                {
                    string failReplyMessage = $"{ActivityBundle.Caller.Name}, I've failed to find requested image, however, have a look at this one!";
                    replies.Add(ActivityBundle.Activity.CreateReply(failReplyMessage));
                    image = GetRandomCardImage();
                }
            }

            ThumbnailCard card = new ThumbnailCard();

            if (!string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                card.Title = ActivityBundle.Message.CommandArg;
            }

            List<CardImage> images = new List<CardImage>();
            images.Add(image);
            card.Images = images;
            imageReply.Attachments.Add(card.ToAttachment());

            replies.Add(imageReply);

            return replies;
        }

        private CardImage GetRandomCardImage()
        {
            int index = new Random().Next(subCommands.Count);

            return new CardImage()
            {
                Url = ImageFolder + subCommands.ElementAt(index).Value
            };
        }

        private bool TryGetRequestedCardImage(out CardImage image)
        {
            string imageName;

            if (!subCommands.TryGetValue(ActivityBundle.Message.SubCommand, out imageName))
            {
                image = null;
                return false;
            }

            image = new CardImage()
            {
                Url = ImageFolder + imageName
            };

            return true;
        }

        private void InitializeSubCommands()
        {
            // TODO : get rid of this ASAP
            subCommands = new Dictionary<string, string>()
            {
                {"kappa", "lirikAppa.jpg"},
                {"b", "lirikB.jpg"},
                {"bb", "lirikBB.jpg"},
                {"blind", "lirikBLIND.jpg"},
                {"c", "lirikC.jpg"},
                {"champ", "lirikCHAMP.jpg"},
                {"clap", "lirikCLAP.jpg"},
                {"crash", "lirikCRASH.jpg"},
                {"d", "lirikD.jpg"},
                {"dj", "lirikDJ.jpg"},
                {"f", "lirikF.jpg"},
                {"fat", "lirikFAT.jpg"},
                {"feels", "lirikFEELS.jpg"},
                {"gasm", "lirikGasm.jpg"},
                {"good", "lirikGOOD.jpg"},
                {"goty", "lirikGOTY.jpg"},
                {"great", "lirikGREAT.jpg"},
                {"h", "lirikH.jpg"},
                {"hold", "lirikHOLD.jpg"},
                {"hug", "lirikHug.jpg"},
                {"hype", "lirikHYPE.jpg"},
                {"l", "lirikL.jpg"},
                {"lewd", "lirikLEWD.jpg"},
                {"loot", "lirikLOOT.jpg"},
                {"lul", "lirikLUL.jpg"},
                {"m", "lirikM.jpg"},
                {"meow", "lirikMEOW.jpg"},
                {"mlg", "lirikMLG.jpg"},
                {"n", "lirikN.jpg"},
                {"nice", "lirikNICE.jpg"},
                {"non", "lirikNON.jpg"},
                {"not", "lirikNOT.jpg"},
                {"o", "lirikO.jpg"},
                {"obese", "lirikOBESE.jpg"},
                {"ohgod", "lirikOHGOD.jpg"},
                {"p", "lirikP.jpg"},
                {"pool", "lirikPOOL.jpg"},
                {"poop", "lirikPOOP.jpg"},
                {"puke", "lirikPUKE.jpg"},
                {"rekt", "lirikREKT.jpg"},
                {"rip", "lirikRIP.jpg"},
                {"salt", "lirikSALT.jpg"},
                {"scared", "lirikSCARED.jpg"},
                {"shucks", "lirikSHUCKS.jpg"},
                {"ten", "lirikTEN.jpg"},
                {"thump", "lirikThump.jpg"},
                {"truck", "lirikTRUCK.jpg"},
                {"w", "lirikW.jpg"},
                {"wc", "lirikWc.jpg"}
            };
        }
    }
}
