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
                return CreateSingleReplyCollection($"{ActivityBundle.Caller.Name}, usage: [image name] [optional text]");
            }

            string pictureName = GetRequestedPictureName();

            ImagePath requestedImage = store.GetAllPictures().FirstOrDefault(p => p.DisplayName.Equals(pictureName, StringComparison.CurrentCultureIgnoreCase));

            if (requestedImage == null)
            {
                return CreateSingleReplyCollection($"Picture '{pictureName}' not found");
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
                return CreateSingleReplyCollection("Store is empty");
            }

            for (int i = 0; i < allPictures.Count; i++)
            {
                sb.AppendLine($@"\#{i} {allPictures[i].DisplayName}");
            }

            return CreateSingleReplyCollection(sb.ToString());
        }

        private IEnumerable<Activity> GetPictureStoreStats()
        {
            PictureStoreStats stats = store.GetStats();

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine($"Current Pictures Number: {stats.CurrentPicturesLoad}");
            sb.AppendLine($"Max Pictures Number: {stats.MaxPicturesLoad}");
            sb.AppendLine($"Store is full on {stats.StoreLoadPercentage.ToString("0.00")}%");

            return CreateSingleReplyCollection(sb.ToString());
        }

        private System.Text.RegularExpressions.Match DetermineUrl()
        {
            return System.Text.RegularExpressions.Regex.Match(ActivityBundle.Message.CommandArg, "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\">[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled, TimeSpan.FromSeconds(5));
        }

        private IEnumerable<Activity> SavePicture()
        {
            System.Text.RegularExpressions.Match imageUrlMatch = DetermineUrl();
            if (!imageUrlMatch.Success)
            {
                return CreateSingleReplyCollection("Failed to determine image url");
            }

            string skypeUrl = imageUrlMatch.Value;
            string normalizedUrl = imageUrlMatch.Groups[1].Value;

            Uri imageUri = new Uri(normalizedUrl, UriKind.Absolute);

            string imageName;
            int urlPosition = ActivityBundle.Message.CommandArg.IndexOf(skypeUrl);
            if (urlPosition == 0)
            {
                imageName = ActivityBundle.Message.CommandArg.Substring(skypeUrl.Length).Trim();
            }
            else
            {
                imageName = ActivityBundle.Message.CommandArg.Substring(0, urlPosition).Trim();
            }

            AddImageResult saveResult = store.Add(imageName, imageUri);

            if (saveResult == null || !saveResult.Success)
            {
                return CreateSingleReplyCollection($"Image was not saved. Error: {saveResult.Error}");
            }

            return CreateSingleReplyCollection($"Image {saveResult.Image.FullName} has been successfully saved");
        }

        private IEnumerable<Activity> DeletePicture()
        {
            string pictureName = GetRequestedPictureName();

            DeleteImageResult deleteResult = store.Delete(pictureName);

            if (deleteResult.Success)
            {
                return CreateSingleReplyCollection($"Image '{pictureName}' has been deleted");
            }
            else
            {
                return CreateSingleReplyCollection(deleteResult.Error);
            }
        }

        private IEnumerable<Activity> ClearPictures()
        {
            DeleteImageResult result = store.Clear();

            if (result.Success)
            {
                return CreateSingleReplyCollection("Store has been cleared out");
            }
            else
            {
                return CreateSingleReplyCollection($"Failed to clear the store. Error message: {result.Error}");
            }
        }

        private void InitializeSubCommands()
        {
            SubCommands = new Dictionary<ICommandInfo, Func<IEnumerable<Activity>>>()
            {
                { new ChatCommandInfo("all"), GetPicturesList },
                { new ChatCommandInfo("add", Group.PictureStoreCM), SavePicture },
                { new ChatCommandInfo("delete", Group.PictureStoreCM), DeletePicture },
                { new ChatCommandInfo("clear", Group.PictureStoreCM), ClearPictures },
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
