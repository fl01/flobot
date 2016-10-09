using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Identity;
using Flobot.Messages.Commands;
using Flobot.Messages.LocalHandlers.PictureStore;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.LocalHandlers
{
    [Permissions(Role.User)]
    [Message("shared custom pictures", Section.Default, "PictureStore", "ps")]
    public class PictureStoreHandler : MessageHandlerBase
    {
        private IPictureStore store;

        public PictureStoreHandler()
        {
            store = IoC.Container.Resolve<IPictureStore>();

            InitializeSubCommands();
        }

        protected override IEnumerable<Activity> CreateReplies(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.SubCommand))
            {
                return CreateHelpReplies(activityBundle);
            }

            var command = GetPermittedSubCommand(activityBundle, activityBundle.Message.SubCommand);

            if (command.Value == null)
            {
                return GetInvalidSubCommandReply(activityBundle);
            }

            return command.Value(activityBundle);
        }

        private IEnumerable<Activity> GetExactPicture(ActivityBundle activityBundle)
        {
            if (string.IsNullOrEmpty(activityBundle.Message.CommandArg))
            {
                return CreateSingleReplyCollection(activityBundle, $"{activityBundle.Caller.Name}, usage: [image name] [optional text]");
            }

            string pictureName = GetRequestedPictureName(activityBundle.Message.CommandArg);

            ImagePath requestedImage = store.GetAllPictures().FirstOrDefault(p => p.DisplayName.Equals(pictureName, StringComparison.CurrentCultureIgnoreCase));

            if (requestedImage == null)
            {
                return CreateSingleReplyCollection(activityBundle, $"Picture '{pictureName}' not found");
            }

            string cardText = GetRequestedPictureText(activityBundle.Message.CommandArg);
            ThumbnailCard card = CreateThumbnailCard(cardText);
            CardImage image = CreateCardImage(requestedImage);
            card.Images.Add(image);

            var cardReply = CreateThumbnailCardReply(activityBundle, card);

            return new[] { cardReply };
        }

        private CardImage CreateCardImage(ImagePath image)
        {
            return new CardImage()
            {
                Url = image.WebPath
            };
        }

        private IEnumerable<Activity> GetPicturesList(ActivityBundle activityBundle)
        {
            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            IList<ImagePath> allPictures = store.GetAllPictures();

            if (!allPictures.Any())
            {
                return CreateSingleReplyCollection(activityBundle, "Store is empty");
            }

            for (int i = 0; i < allPictures.Count; i++)
            {
                sb.AppendLine($@"\#{i} {allPictures[i].DisplayName}");
            }

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }

        private IEnumerable<Activity> GetPictureStoreStats(ActivityBundle activityBundle)
        {
            PictureStoreStats stats = store.GetStats();

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine($"Current Pictures Number: {stats.CurrentPicturesLoad}");
            sb.AppendLine($"Max Pictures Number: {stats.MaxPicturesLoad}");
            sb.AppendLine($"Store is full on {stats.StoreLoadPercentage.ToString("0.00")}%");

            return CreateSingleReplyCollection(activityBundle, sb.ToString());
        }

        private System.Text.RegularExpressions.Match DetermineUrl(string commandArg)
        {
            return System.Text.RegularExpressions.Regex.Match(commandArg, "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\">[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled, TimeSpan.FromSeconds(5));
        }

        private IEnumerable<Activity> SavePicture(ActivityBundle activityBundle)
        {
            System.Text.RegularExpressions.Match imageUrlMatch = DetermineUrl(activityBundle.Message.CommandArg);
            if (!imageUrlMatch.Success)
            {
                return CreateSingleReplyCollection(activityBundle, "Failed to determine image url");
            }

            string skypeUrl = imageUrlMatch.Value;
            string normalizedUrl = imageUrlMatch.Groups[1].Value;

            Uri imageUri = new Uri(normalizedUrl, UriKind.Absolute);

            string imageName;
            int urlPosition = activityBundle.Message.CommandArg.IndexOf(skypeUrl);
            if (urlPosition == 0)
            {
                imageName = activityBundle.Message.CommandArg.Substring(skypeUrl.Length).Trim();
            }
            else
            {
                imageName = activityBundle.Message.CommandArg.Substring(0, urlPosition).Trim();
            }

            AddImageResult saveResult = store.Add(imageName, imageUri);

            if (saveResult == null || !saveResult.Success)
            {
                return CreateSingleReplyCollection(activityBundle, $"Image was not saved. Error: {saveResult.Error}");
            }

            return CreateSingleReplyCollection(activityBundle, $"Image {saveResult.Image.FullName} has been successfully saved");
        }

        private IEnumerable<Activity> DeletePicture(ActivityBundle activityBundle)
        {
            string pictureName = GetRequestedPictureName(activityBundle.Message.CommandArg);

            DeleteImageResult deleteResult = store.Delete(pictureName);

            string text = deleteResult.Success ? $"Image '{pictureName}' has been deleted" : deleteResult.Error;
            return CreateSingleReplyCollection(activityBundle, text);
        }

        private IEnumerable<Activity> ClearPictures(ActivityBundle activityBundle)
        {
            DeleteImageResult result = store.Clear();

            string text = result.Success ? "Store has been cleared out" : $"Failed to clear the store. Error message: {result.Error}";
            return CreateSingleReplyCollection(activityBundle, text);
        }

        private void InitializeSubCommands()
        {
            SubCommands.Add(new ChatCommandInfo("all"), GetPicturesList);
            SubCommands.Add(new ChatCommandInfo("add", Group.PictureStoreCM), SavePicture);
            SubCommands.Add(new ChatCommandInfo("delete", Group.PictureStoreCM), DeletePicture);
            SubCommands.Add(new ChatCommandInfo("clear", Group.PictureStoreCM), ClearPictures);
            SubCommands.Add(new ChatCommandInfo("stats"), GetPictureStoreStats);
            SubCommands.Add(new ChatCommandInfo("get"), GetExactPicture);
        }

        private string GetRequestedPictureName(string commandArg)
        {
            return commandArg.Split(' ')?.First();
        }

        private string GetRequestedPictureText(string commandArg)
        {
            int firstArgEnd = commandArg.IndexOf(' ');
            if (firstArgEnd < 0)
            {
                return string.Empty;
            }

            return commandArg.Substring(firstArgEnd).Trim();
        }
    }
}
