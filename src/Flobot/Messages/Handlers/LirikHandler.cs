using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("just some nice pictures with optional text", Section.Default, "lirik", "lrk")]
    public class LirikHandler : MessageHandlerBase
    {
        public LirikHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                return GetRandomImageReply();
            }

            var commandInfo = GetPermittedSubCommand(ActivityBundle.Message.SubCommand);

            if (commandInfo.Value == null)
            {
                string failMessage = $"{ActivityBundle.Caller.Name}, I've failed to find requested image, however, have a look at this one!";
                var failMessageReply = ActivityBundle.Activity.CreateReply(failMessage);
                var randomImageReply = GetRandomImageReply();

                return new[] { failMessageReply }.Concat(randomImageReply);
            }

            return commandInfo.Value();
        }

        private Activity CreateImageReply(string imageName)
        {
            Activity imageReply = ActivityBundle.Activity.CreateReply();
            imageReply.AttachmentLayout = "carousel";

            CardImage image;
            if (!TryGetRequestedCardImage(imageName, out image))
            {
                string errorMessage = $"Image {imageName} is missing";
                Logger.Error(errorMessage);
                return ActivityBundle.Activity.CreateReply(errorMessage);
            }

            ThumbnailCard card = CreateThumbnailCard(ActivityBundle.Message.CommandArg);

            card.Images.Add(image);
            imageReply.Attachments.Add(card.ToAttachment());

            return imageReply;
        }

        private IEnumerable<Activity> GetRandomImageReply()
        {
            var allImageReplies = GetPermittedSubCommands();
            return allImageReplies.ElementAt(new Random().Next(allImageReplies.Count())).Value();
        }

        private bool TryGetRequestedCardImage(string imageName, out CardImage image)
        {
            ImagePath imagePath = new ImagePath(imageName, "Lirik");

            if (!imagePath.Exists)
            {
                image = null;
                return false;
            }

            image = new CardImage()
            {
                Url = imagePath.WebPath
            };

            return true;
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("kappa"), GetLirikKappaReply },
                { new ChatCommandInfo("b"), GetLirikBReply },
                { new ChatCommandInfo("bb"), GetLirikBBReply },
                { new ChatCommandInfo("blind"), GetLirikBlindReply },
                { new ChatCommandInfo("c"), GetLirikCReply },
                { new ChatCommandInfo("champ"), GetLirikChampReply },
                { new ChatCommandInfo("clap"), GetLirikClapReply },
                { new ChatCommandInfo("crash"), GetLirikCrashReply },
                { new ChatCommandInfo("d"), GetLirikDReply },
                { new ChatCommandInfo("dj"), GetLirikDJReply },
                { new ChatCommandInfo("f"), GetLirikFReply },
                { new ChatCommandInfo("fat"), GetLirikFatReply },
                { new ChatCommandInfo("feels"), GetLirikFeelsReply },
                { new ChatCommandInfo("gasm"), GetLirikGasmReply },
                { new ChatCommandInfo("good"), GetLirikGoodReply },
                { new ChatCommandInfo("goty"), GetLirikGotyReply },
                { new ChatCommandInfo("great"), GetLirikGreatReply },
                { new ChatCommandInfo("h"), GetLirikHReply },
                { new ChatCommandInfo("hold"), GetLirikHoldReply },
                { new ChatCommandInfo("hug"), GetLirikHugReply },
                { new ChatCommandInfo("hype"), GetLirikHypeReply },
                { new ChatCommandInfo("l"), GetLirikLReply },
                { new ChatCommandInfo("lewd"), GetLirikLewdReply },
                { new ChatCommandInfo("loot"), GetLirikLootReply },
                { new ChatCommandInfo("lul"), GetLirikLulReply },
                { new ChatCommandInfo("m"), GetLirikMReply },
                { new ChatCommandInfo("meow"), GetLirikMeowReply },
                { new ChatCommandInfo("mlg"), GetLirikMlgReply },
                { new ChatCommandInfo("n"), GetLirikNReply },
                { new ChatCommandInfo("nice"), GetLirikNiceReply },
                { new ChatCommandInfo("non"), GetLirikNonReply },
                { new ChatCommandInfo("not"), GetLirikNotReply },
                { new ChatCommandInfo("o"), GetLirikOReply },
                { new ChatCommandInfo("obese"), GetLirikObeseReply },
                { new ChatCommandInfo("og"), GetLirikOgReply },
                { new ChatCommandInfo("ohgod"), GetLirikOhgodReply },
                { new ChatCommandInfo("p"), GetLirikPReply },
                { new ChatCommandInfo("pool"), GetLirikPoolReply },
                { new ChatCommandInfo("poop"), GetLirikPoopReply },
                { new ChatCommandInfo("puke"), GetLirikPukeReply },
                { new ChatCommandInfo("rekt"), GetLirikRektReply },
                { new ChatCommandInfo("rip"), GetLirikRipReply },
                { new ChatCommandInfo("salt"), GetLirikSaltReply },
                { new ChatCommandInfo("scared"), GetLirikScaredReply },
                { new ChatCommandInfo("shucks"), GetLirikShucksReply },
                { new ChatCommandInfo("ten"), GetLirikTenReply },
                { new ChatCommandInfo("thump"), GetLirikThumpReply },
                { new ChatCommandInfo("truck"), GetLirikTruckReply },
                { new ChatCommandInfo("w"), GetLirikWReply },
                { new ChatCommandInfo("wc"), GetLirikWcReply }
            };
        }

        #region Image Methods

        private IEnumerable<Activity> GetLirikKappaReply()
        {
            Activity imageReply = CreateImageReply("lirikAppa.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBReply()
        {
            Activity imageReply = CreateImageReply("lirikB.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBBReply()
        {
            Activity imageReply = CreateImageReply("lirikBB.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBlindReply()
        {
            Activity imageReply = CreateImageReply("lirikBLIND.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikCReply()
        {
            Activity imageReply = CreateImageReply("lirikC.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikChampReply()
        {
            Activity imageReply = CreateImageReply("lirikCHAMP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikClapReply()
        {
            Activity imageReply = CreateImageReply("lirikCLAP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikCrashReply()
        {
            Activity imageReply = CreateImageReply("lirikCRASH.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikDReply()
        {
            Activity imageReply = CreateImageReply("lirikD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikDJReply()
        {
            Activity imageReply = CreateImageReply("lirikDJ.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFReply()
        {
            Activity imageReply = CreateImageReply("lirikF.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFatReply()
        {
            Activity imageReply = CreateImageReply("lirikFAT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFeelsReply()
        {
            Activity imageReply = CreateImageReply("lirikFEELS.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGasmReply()
        {
            Activity imageReply = CreateImageReply("lirikGasm.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGoodReply()
        {
            Activity imageReply = CreateImageReply("lirikGOOD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGotyReply()
        {
            Activity imageReply = CreateImageReply("lirikGOTY.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGreatReply()
        {
            Activity imageReply = CreateImageReply("lirikGREAT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHReply()
        {
            Activity imageReply = CreateImageReply("lirikH.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHoldReply()
        {
            Activity imageReply = CreateImageReply("lirikHOLD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHugReply()
        {
            Activity imageReply = CreateImageReply("lirikHug.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHypeReply()
        {
            Activity imageReply = CreateImageReply("lirikHYPE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLReply()
        {
            Activity imageReply = CreateImageReply("lirikL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLewdReply()
        {
            Activity imageReply = CreateImageReply("lirikLEWD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLootReply()
        {
            Activity imageReply = CreateImageReply("lirikLOOT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLulReply()
        {
            Activity imageReply = CreateImageReply("lirikLUL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMReply()
        {
            Activity imageReply = CreateImageReply("lirikM.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMeowReply()
        {
            Activity imageReply = CreateImageReply("lirikMEOW.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMlgReply()
        {
            Activity imageReply = CreateImageReply("lirikMLG.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNReply()
        {
            Activity imageReply = CreateImageReply("lirikN.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNiceReply()
        {
            Activity imageReply = CreateImageReply("lirikNICE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNonReply()
        {
            Activity imageReply = CreateImageReply("lirikNON.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNotReply()
        {
            Activity imageReply = CreateImageReply("lirikNOT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOReply()
        {
            Activity imageReply = CreateImageReply("lirikO.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikObeseReply()
        {
            Activity imageReply = CreateImageReply("lirikOBESE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOgReply()
        {
            Activity imageReply = CreateImageReply("lirikOG.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOhgodReply()
        {
            Activity imageReply = CreateImageReply("lirikOHGOD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPReply()
        {
            Activity imageReply = CreateImageReply("lirikP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPoolReply()
        {
            Activity imageReply = CreateImageReply("lirikPOOL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPoopReply()
        {
            Activity imageReply = CreateImageReply("lirikPOOP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPukeReply()
        {
            Activity imageReply = CreateImageReply("lirikPUKE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikRektReply()
        {
            Activity imageReply = CreateImageReply("lirikREKT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikRipReply()
        {
            Activity imageReply = CreateImageReply("lirikRIP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikSaltReply()
        {
            Activity imageReply = CreateImageReply("lirikSALT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikScaredReply()
        {
            Activity imageReply = CreateImageReply("lirikSCARED.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikShucksReply()
        {
            Activity imageReply = CreateImageReply("lirikSHUCKS.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikTenReply()
        {
            Activity imageReply = CreateImageReply("lirikTEN.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikThumpReply()
        {
            Activity imageReply = CreateImageReply("lirikThump.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikTruckReply()
        {
            Activity imageReply = CreateImageReply("lirikTRUCK.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikWReply()
        {
            Activity imageReply = CreateImageReply("lirikW.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikWcReply()
        {
            Activity imageReply = CreateImageReply("lirikWc.jpg");

            return new[] { imageReply };
        }

        #endregion // Image Methods
    }
}
