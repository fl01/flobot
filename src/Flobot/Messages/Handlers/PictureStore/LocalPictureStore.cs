﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Flobot.Common;
using Flobot.Logging;

namespace Flobot.Messages.Handlers.PictureStore
{
    public class LocalPictureStore : IPictureStore
    {
        private const string ImageSubFolder = "PictureStore";
        private const string ImageExtension = "jpg";
        private const int MaxPictures = 10;
        private ILog logger;

        private string ImageFolderPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath($@"~\Images\{ImageSubFolder}\");
            }
        }

        public LocalPictureStore(ILoggingService loggingService)
        {
            logger = loggingService.GetLogger(this);
        }

        public IList<ImagePath> GetAllPictures()
        {
            return InternalGetAllPictures();
        }

        public PictureStoreStats GetStats()
        {
            return InternalGetStoreStats();
        }

        public AddImageResult Add(string imageName, Uri imageUrl)
        {
            if (!GetIsWebResourceExists(imageUrl))
            {
                return AddImageResult.CreateFailResult("Unable to locate url");
            }

            if (GetIsImageExists(imageName))
            {
                return AddImageResult.CreateFailResult($"Image with name '{imageName}' already exists");
            }

            string downloadedImage = DownloadImage(imageUrl, imageName);

            if (string.IsNullOrEmpty(downloadedImage))
            {
                return AddImageResult.CreateFailResult("Failed to download or save the image");
            }

            string downloadedImageName = Path.GetFileName(downloadedImage);

            var imagePath = new ImagePath(downloadedImageName, ImageSubFolder);
            return AddImageResult.CreateSuccessResult(imagePath);
        }

        private List<ImagePath> InternalGetAllPictures()
        {
            try
            {
                return Directory.GetFiles(ImageFolderPath)
                    .Where(f => f.EndsWith("." + ImageExtension, StringComparison.CurrentCultureIgnoreCase))
                    .Select(f => new FileInfo(f))
                    .Select(x => new ImagePath(x.Name, ImageSubFolder))
                    .ToList();
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Error(ex);
                return new List<ImagePath>();
            }
        }

        private PictureStoreStats InternalGetStoreStats()
        {
            return new PictureStoreStats()
            {
                MaxPicturesLoad = MaxPictures,
                CurrentPicturesLoad = InternalGetAllPictures().Count
            };
        }

        private bool GetIsStoreFull()
        {
            PictureStoreStats stats = InternalGetStoreStats();

            return stats.CurrentPicturesLoad < stats.MaxPicturesLoad;
        }

        private bool GetIsImageExists(string imageName)
        {
            return InternalGetAllPictures().Any(i => i.DisplayName.Equals(imageName, StringComparison.CurrentCultureIgnoreCase));
        }

        private string DownloadImage(Uri imageUrl, string imageName)
        {
            try
            {
                using (var tempFolder = new TempFolder())
                {
                    string expectedFile = Path.Combine(tempFolder.FolderPath, Guid.NewGuid().ToString("D"));

                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(imageUrl, expectedFile);
                    }

                    if (!EnsureImage(expectedFile))
                    {
                        return null;
                    }

                    string savedImagePath = Path.Combine(ImageFolderPath, imageName + "." + ImageExtension);

                    // this should never occur
                    // TODO : thread safe file manager
                    if (File.Exists(savedImagePath))
                    {
                        File.Delete(savedImagePath);
                    }

                    File.Move(expectedFile, savedImagePath);

                    return savedImagePath;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        private bool EnsureImage(string filePath)
        {
            FileStream stream = null;
            try
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return stream.IsImage();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
            finally
            {
                stream?.Close();
            }
        }

        private bool GetIsWebResourceExists(Uri resourceUrl)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(resourceUrl);
                request.Method = RequestType.Head.ToString();
                request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                logger.Error($"Resource {resourceUrl.AbsoluteUri} doesn't exist. Status code: {((HttpWebResponse)ex.Response).StatusCode}");
                return false;
            }
        }
    }
}
