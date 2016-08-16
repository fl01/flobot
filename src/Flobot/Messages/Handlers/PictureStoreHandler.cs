using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.Handlers.PictureStore;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("shared custom pictures", Section.Default, "PictureStore", "ps")]
    public class PictureStoreHandler : MessageHandlerBase
    {
        private IPictureStore store;

        public PictureStoreHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
            store = IoC.Container.Resolve<IPictureStore>();

            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.SubCommand))
            {
                return CreateHelpReplies();
            }

            var command = GetPermittedSubCommand(ActivityBundle.Message.SubCommand);

            if (command.Value == null)
            {
                return GetInvalidSubCommandReply();
            }

            return command.Value();
        }

        private IEnumerable<Activity> GetExactPicture()
        {
            if (string.IsNullOrEmpty(ActivityBundle.Message.CommandArg))
            {
                return new[] { ActivityBundle.Activity.CreateReply($"{ActivityBundle.Caller.Name}, usage: [image name] [optional text]") };
            }

            string pictureName = GetRequestedPictureName();

            ImagePath requestedImage = store.GetAllPictures().FirstOrDefault(p => p.DisplayName.Equals(pictureName, StringComparison.CurrentCultureIgnoreCase));

            if (requestedImage == null)
            {
                return new[] { ActivityBundle.Activity.CreateReply($"Picture '{pictureName}' not found") };
            }

            string cardText = GetRequestedPictureText();
            ThumbnailCard card = CreateThumbnailCard(cardText);
            CardImage image = CreateCardImage(requestedImage);
            card.Images.Add(image);

            var cardReply = CreateThumbnailCardReply(card);

            return new[] { cardReply };
        }

        private CardImage CreateCardImage(ImagePath image)
        {
            return new CardImage()
            {
                Url = image.WebPath
            };
        }

        private IEnumerable<Activity> GetPicturesList()
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            IList<ImagePath> allPictures = store.GetAllPictures();

            if (!allPictures.Any())
            {
                return new[] { ActivityBundle.Activity.CreateReply("Store is empty") };
            }

            for (int i = 0; i < allPictures.Count; i++)
            {
                sb.AppendLine($@"\#{i} {allPictures[i].DisplayName}");
            }

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        private IEnumerable<Activity> GetPictureStoreStats()
        {
            PictureStoreStats stats = store.GetStats();

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine($"Current Pictures Number: {stats.CurrentPicturesLoad}");
            sb.AppendLine($"Max Pictures Number: {stats.MaxPicturesLoad}");
            sb.AppendLine($"Store is full on {stats.StoreLoadPercentage.ToString("0.00")}%");

            return new[] { ActivityBundle.Activity.CreateReply(sb.ToString()) };
        }

        private IEnumerable<Activity> SavePicture()
        {
            Uri imageUri = null;

            bool urlIsFound = ActivityBundle.Message.CommandArg.Split(' ').Any(i => Uri.TryCreate(i, UriKind.Absolute, out imageUri) && imageUri.Scheme == Uri.UriSchemeHttp);
            if (!urlIsFound)
            {
                return new[] { ActivityBundle.Activity.CreateReply("Failed to determine image url") };
            }

            string imageName;
            int urlPosition = ActivityBundle.Message.CommandArg.IndexOf(imageUri.AbsoluteUri);
            if (urlPosition == 0)
            {
                imageName = ActivityBundle.Message.CommandArg.Substring(imageUri.AbsoluteUri.Length).Trim();
            }
            else
            {
                imageName = ActivityBundle.Message.CommandArg.Substring(0, urlPosition).Trim();
            }

            Debug.Assert(imageUri != null);

            AddImageResult saveResult = store.Add(imageName, imageUri);

            if (saveResult == null || !saveResult.Success)
            {
                return new[] { ActivityBundle.Activity.CreateReply($"Image was not saved. Error: {saveResult.Error}") };
            }

            return new[] { ActivityBundle.Activity.CreateReply($"Image {saveResult.Image.FullName} has been successfully saved") };
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("all"), GetPicturesList },
                { new ChatCommandInfo("add", Group.PictureStoreCM), SavePicture },
                { new ChatCommandInfo("stats"), GetPictureStoreStats },
                { new ChatCommandInfo("get"), GetExactPicture }
            };
        }

        private string GetRequestedPictureName()
        {
            return ActivityBundle.Message.CommandArg.Split(' ')?.First();
        }

        private string GetRequestedPictureText()
        {
            int firstArgEnd = ActivityBundle.Message.CommandArg.IndexOf(' ');
            if (firstArgEnd < 0)
            {
                return string.Empty;
            }

            return ActivityBundle.Message.CommandArg.Substring(firstArgEnd).Trim();
        }
    }
}
