using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message(Section.Default, "lirik", "lrk")]
    public class LirikHandler : MessageHandlerBase
    {
        private Dictionary<string, string> imageContainer;

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

            ThumbnailCard card = CreateThumbnailCard();

            if (!string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                // current version of Skype (7.26.01) wraps thumbnail title if it is longer than 26 symbols or 2 lines
                // so, let's display text bigger if we can (ᵔ◡ᵔ)
                if (ActivityBundle.Message.CommandArg.Length <= 26 && ActivityBundle.Message.CommandArg.Split('\n').Length <= 2)
                {
                    card.Title = ActivityBundle.Message.CommandArg;
                }
                else
                {
                    card.Text = ActivityBundle.Message.CommandArg;
                }
            }

            card.Images.Add(image);
            imageReply.Attachments.Add(card.ToAttachment());

            replies.Add(imageReply);

            return replies;
        }

        private CardImage GetRandomCardImage()
        {
            int index = new Random().Next(imageContainer.Count);

            return new CardImage()
            {
                Url = ImageFolder + imageContainer.ElementAt(index).Value
            };
        }

        private bool TryGetRequestedCardImage(out CardImage image)
        {
            string imageName;

            ICommandInfo subCommand = GetPermittedSubCommands()
                .FirstOrDefault(x => x.Key.Name.Equals(ActivityBundle.Message.SubCommand, StringComparison.CurrentCultureIgnoreCase))
                .Key;

            if (subCommand == null || !imageContainer.TryGetValue(subCommand.Name, out imageName))
            {
                Logger.Debug($"Invalid sub-command {ActivityBundle.Message.SubCommand}");
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
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("kappa"), CreateReplies },
                { new ChatCommandInfo("b"), CreateReplies },
                { new ChatCommandInfo("bb"), CreateReplies },
                { new ChatCommandInfo("blind"), CreateReplies },
                { new ChatCommandInfo("c"), CreateReplies },
                { new ChatCommandInfo("champ"), CreateReplies },
                { new ChatCommandInfo("clap"), CreateReplies },
                { new ChatCommandInfo("crash"), CreateReplies },
                { new ChatCommandInfo("d"), CreateReplies },
                { new ChatCommandInfo("dj"), CreateReplies },
                { new ChatCommandInfo("f"), CreateReplies },
                { new ChatCommandInfo("fat"), CreateReplies },
                { new ChatCommandInfo("feels"), CreateReplies },
                { new ChatCommandInfo("gasm"), CreateReplies },
                { new ChatCommandInfo("good"), CreateReplies },
                { new ChatCommandInfo("goty"), CreateReplies },
                { new ChatCommandInfo("great"), CreateReplies },
                { new ChatCommandInfo("h"), CreateReplies },
                { new ChatCommandInfo("hold"), CreateReplies },
                { new ChatCommandInfo("hug"), CreateReplies },
                { new ChatCommandInfo("hype"), CreateReplies },
                { new ChatCommandInfo("l"), CreateReplies },
                { new ChatCommandInfo("lewd"), CreateReplies },
                { new ChatCommandInfo("loot"), CreateReplies },
                { new ChatCommandInfo("lul"), CreateReplies },
                { new ChatCommandInfo("m"), CreateReplies },
                { new ChatCommandInfo("meow"), CreateReplies },
                { new ChatCommandInfo("mlg"), CreateReplies },
                { new ChatCommandInfo("n"), CreateReplies },
                { new ChatCommandInfo("nice"), CreateReplies },
                { new ChatCommandInfo("non"), CreateReplies },
                { new ChatCommandInfo("not"), CreateReplies },
                { new ChatCommandInfo("o"), CreateReplies },
                { new ChatCommandInfo("obese"), CreateReplies },
                { new ChatCommandInfo("ohgod"), CreateReplies },
                { new ChatCommandInfo("p"), CreateReplies },
                { new ChatCommandInfo("pool"), CreateReplies },
                { new ChatCommandInfo("poop"), CreateReplies },
                { new ChatCommandInfo("puke"), CreateReplies },
                { new ChatCommandInfo("rekt"), CreateReplies },
                { new ChatCommandInfo("rip"), CreateReplies },
                { new ChatCommandInfo("salt"), CreateReplies },
                { new ChatCommandInfo("scared"), CreateReplies },
                { new ChatCommandInfo("shucks"), CreateReplies },
                { new ChatCommandInfo("ten"), CreateReplies },
                { new ChatCommandInfo("thump"), CreateReplies },
                { new ChatCommandInfo("truck"), CreateReplies },
                { new ChatCommandInfo("w"), CreateReplies },
                { new ChatCommandInfo("wc"), CreateReplies }
            };

            // TODO : get rid of this ASAP
            imageContainer = new Dictionary<string, string>()
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
