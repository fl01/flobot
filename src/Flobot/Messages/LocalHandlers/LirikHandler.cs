using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("just some nice pictures with optional text", Section.Default, "lirik", "lrk")]
    public class LirikHandler : MessageHandlerBase
    {
        public LirikHandler()
        {
            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                return GetRandomImageReply(activityBundle);
            }

            var commandInfo = GetPermittedSubCommand(activityBundle, activityBundle.Message.SubCommand);

            if (commandInfo.Value == null)
            {
                string failMessage = $"{activityBundle.Caller.Name}, I've failed to find requested image, however, have a look at this one!";
                var failMessageReply = CreateSingleReply(activityBundle, failMessage);
                var randomImageReply = GetRandomImageReply(activityBundle);

                return new[] { failMessageReply }.Concat(randomImageReply);
            }

            return commandInfo.Value(activityBundle);
        }

        private Activity CreateImageReply(ActivityBundle bundle, string imageName)
        {
            CardImage image;
            if (!TryGetRequestedCardImage(imageName, out image))
            {
                string errorMessage = $"Image {imageName} is missing";
                Logger.Error(errorMessage);
                return CreateSingleReply(bundle, errorMessage);
            }

            ThumbnailCard card = CreateThumbnailCard(bundle.Message.CommandArg);
            card.Images.Add(image);

            Activity imageReply = CreateThumbnailCardReply(bundle, card);

            return imageReply;
        }

        private IEnumerable<Activity> GetRandomImageReply(ActivityBundle bundle)
        {
            var allImageReplies = GetPermittedSubCommands(bundle);
            return allImageReplies
                .ElementAt(new Random().Next(allImageReplies.Count()))
                .Value(bundle);
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
            SubCommands.Add(new ChatCommandInfo("kappa"), GetLirikKappaReply);
            SubCommands.Add(new ChatCommandInfo("b"), GetLirikBReply);
            SubCommands.Add(new ChatCommandInfo("bb"), GetLirikBBReply);
            SubCommands.Add(new ChatCommandInfo("blind"), GetLirikBlindReply);
            SubCommands.Add(new ChatCommandInfo("c"), GetLirikCReply);
            SubCommands.Add(new ChatCommandInfo("champ"), GetLirikChampReply);
            SubCommands.Add(new ChatCommandInfo("clap"), GetLirikClapReply);
            SubCommands.Add(new ChatCommandInfo("crash"), GetLirikCrashReply);
            SubCommands.Add(new ChatCommandInfo("d"), GetLirikDReply);
            SubCommands.Add(new ChatCommandInfo("dj"), GetLirikDJReply);
            SubCommands.Add(new ChatCommandInfo("f"), GetLirikFReply);
            SubCommands.Add(new ChatCommandInfo("fat"), GetLirikFatReply);
            SubCommands.Add(new ChatCommandInfo("feels"), GetLirikFeelsReply);
            SubCommands.Add(new ChatCommandInfo("gasm"), GetLirikGasmReply);
            SubCommands.Add(new ChatCommandInfo("good"), GetLirikGoodReply);
            SubCommands.Add(new ChatCommandInfo("goty"), GetLirikGotyReply);
            SubCommands.Add(new ChatCommandInfo("great"), GetLirikGreatReply);
            SubCommands.Add(new ChatCommandInfo("h"), GetLirikHReply);
            SubCommands.Add(new ChatCommandInfo("hold"), GetLirikHoldReply);
            SubCommands.Add(new ChatCommandInfo("hug"), GetLirikHugReply);
            SubCommands.Add(new ChatCommandInfo("hype"), GetLirikHypeReply);
            SubCommands.Add(new ChatCommandInfo("l"), GetLirikLReply);
            SubCommands.Add(new ChatCommandInfo("lewd"), GetLirikLewdReply);
            SubCommands.Add(new ChatCommandInfo("loot"), GetLirikLootReply);
            SubCommands.Add(new ChatCommandInfo("lul"), GetLirikLulReply);
            SubCommands.Add(new ChatCommandInfo("m"), GetLirikMReply);
            SubCommands.Add(new ChatCommandInfo("meow"), GetLirikMeowReply);
            SubCommands.Add(new ChatCommandInfo("mlg"), GetLirikMlgReply);
            SubCommands.Add(new ChatCommandInfo("n"), GetLirikNReply);
            SubCommands.Add(new ChatCommandInfo("nice"), GetLirikNiceReply);
            SubCommands.Add(new ChatCommandInfo("non"), GetLirikNonReply);
            SubCommands.Add(new ChatCommandInfo("not"), GetLirikNotReply);
            SubCommands.Add(new ChatCommandInfo("o"), GetLirikOReply);
            SubCommands.Add(new ChatCommandInfo("obese"), GetLirikObeseReply);
            SubCommands.Add(new ChatCommandInfo("og"), GetLirikOgReply);
            SubCommands.Add(new ChatCommandInfo("ohgod"), GetLirikOhgodReply);
            SubCommands.Add(new ChatCommandInfo("p"), GetLirikPReply);
            SubCommands.Add(new ChatCommandInfo("pool"), GetLirikPoolReply);
            SubCommands.Add(new ChatCommandInfo("poop"), GetLirikPoopReply);
            SubCommands.Add(new ChatCommandInfo("puke"), GetLirikPukeReply);
            SubCommands.Add(new ChatCommandInfo("rekt"), GetLirikRektReply);
            SubCommands.Add(new ChatCommandInfo("rip"), GetLirikRipReply);
            SubCommands.Add(new ChatCommandInfo("salt"), GetLirikSaltReply);
            SubCommands.Add(new ChatCommandInfo("scared"), GetLirikScaredReply);
            SubCommands.Add(new ChatCommandInfo("shucks"), GetLirikShucksReply);
            SubCommands.Add(new ChatCommandInfo("ten"), GetLirikTenReply);
            SubCommands.Add(new ChatCommandInfo("thump"), GetLirikThumpReply);
            SubCommands.Add(new ChatCommandInfo("truck"), GetLirikTruckReply);
            SubCommands.Add(new ChatCommandInfo("w"), GetLirikWReply);
            SubCommands.Add(new ChatCommandInfo("wc"), GetLirikWcReply);
        }

        #region Image Methods

        private IEnumerable<Activity> GetLirikKappaReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikAppa.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikB.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBBReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikBB.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikBlindReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikBLIND.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikCReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikC.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikChampReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikCHAMP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikClapReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikCLAP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikCrashReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikCRASH.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikDReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikDJReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikDJ.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikF.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFatReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikFAT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikFeelsReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikFEELS.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGasmReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikGasm.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGoodReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikGOOD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGotyReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikGOTY.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikGreatReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikGREAT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikH.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHoldReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikHOLD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHugReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikHug.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikHypeReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikHYPE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLewdReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikLEWD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLootReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikLOOT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikLulReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikLUL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikM.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMeowReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikMEOW.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikMlgReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikMLG.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikN.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNiceReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikNICE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNonReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikNON.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikNotReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikNOT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikO.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikObeseReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikOBESE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOgReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikOG.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikOhgodReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikOHGOD.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPoolReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikPOOL.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPoopReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikPOOP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikPukeReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikPUKE.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikRektReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikREKT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikRipReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikRIP.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikSaltReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikSALT.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikScaredReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikSCARED.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikShucksReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikSHUCKS.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikTenReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikTEN.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikThumpReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikThump.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikTruckReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikTRUCK.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikWReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikW.jpg");

            return new[] { imageReply };
        }

        private IEnumerable<Activity> GetLirikWcReply(ActivityBundle bundle)
        {
            Activity imageReply = CreateImageReply(bundle, "lirikWc.jpg");

            return new[] { imageReply };
        }

        #endregion // Image Methods
    }
}
