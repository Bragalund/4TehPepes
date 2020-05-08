using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pepsCrawler.Helpers;
using pepsCrawler.Models;

namespace pepsCrawler.Crawlers
{
    public class FourChanCrawler : I4ChanCrawler
    {
        private const string BaseUrl = "https://boards.4chan.org/wg/";
        private readonly FourChanHttpClient _client;

        public FourChanCrawler(FourChanHttpClient client)
        {
            _client = client;
        }

        public async Task<List<ImageDto>> GetPepesFrom4Chan()
        {
            var images = new List<ImageDto>();
            using var response = await _client.GetAsyncWithNewUserAgent(BaseUrl);
            using (var content = response.Content)
            {
                var firstMainPage = await HtmlHelpers.ParseContentToHtmlDocument(content);
                var chanThreads = firstMainPage.DocumentNode.SelectNodes(XPathConstants.AllThreadsOnMainPage);
                if (chanThreads != null)
                    foreach (var thread in chanThreads)
                    {
                        var linksToThreads =
                            firstMainPage.DocumentNode.SelectNodes(
                                XPathConstants.LinksToThreadsOnMainPage);
                        if (linksToThreads.Count > 1)
                        {
                            // gå inn i hver threadlink, men ikke den første på førstesiden, fordi den er guidelines for forumet.
                            for (var i = 1; i < linksToThreads.Count; i++)
                                await CrawlAndSaveImagesForThread(chanThreads[i]);
                        }
                        else
                        {
                            // Too few images to be a thread
                            // Get all images from main page
                            images = await GetAllImagesAsStreams(thread);
                            await WriteImagesToFile(images);
                        }
                    }
                else
                    Console.WriteLine("No chantreads!");
            }

            return images;
        }

        private async Task<bool> WriteImagesToFile(List<ImageDto> imageDtos)
        {
            try
            {
                foreach (var imageDto in imageDtos) await WriteImageToFile(imageDto);

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            return false;
        }

        private async Task<string> WriteImageToFile(ImageDto imageDto)
        {
            var path = "D:\\Peps\\DownloadedWithScript";
            var img = Image.FromStream(imageDto.ImageStream);
            var isHorizontal = IsHorizontalImage(img);
            if (isHorizontal)
                path += "\\horizontal\\";
            else
                path += "\\portrait\\";

            var pathAndFilename = path + imageDto.ImageName;

            var shouldSaveImage = ShouldSaveImage(isHorizontal, img);
            if (shouldSaveImage)
            {
                Console.WriteLine("saving image to path: " + pathAndFilename);
                img.Save(pathAndFilename, imageDto.ChosenImageFormat);
            }
            else
            {
                Console.WriteLine("Not saving image: " + pathAndFilename);
            }

            await imageDto.ImageStream.FlushAsync();
            imageDto.ImageStream.Close();
            return pathAndFilename;
        }

        private bool ShouldSaveImage(bool isHorizontal, Image image)
        {
            if (isHorizontal)
            {
                if (image.HorizontalResolution < 60 || image.VerticalResolution < 60)
                {
                    Console.WriteLine("HorizontalRes: " + image.HorizontalResolution + " VerticalResolution: " +
                                      image.VerticalResolution);
                    return false;
                }

                if (image.Size.Width < 2300)
                {
                    Console.WriteLine($"image.Size.Width < 2500. {image.Size.Width}\nForkaster.");
                    return false;
                }
            }
            else
            {
                if (image.VerticalResolution < 60 || image.HorizontalResolution < 60)
                {
                    Console.WriteLine("HorizontalRes: " + image.HorizontalResolution + " VerticalResolution: " +
                                      image.VerticalResolution + "\nNot Saving image.");
                    return false;
                }

                if (image.Size.Height < 1700)
                {
                    Console.WriteLine($"image.Size.Height < 1900. {image.Size.Height}\nForkaster.");
                    return false;
                }
            }

            return true;
        }

        private static bool IsHorizontalImage(Image image)
        {
            return image.Height <= image.Width;
        }

        private async Task<bool> CrawlAndSaveImagesForThread(HtmlNode chanThread)
        {
            var threadUrl = GetThreadUrl(chanThread);
            var imagesFromThread = await GetImagesFromThread(threadUrl);
            var writeWentWell = await WriteImagesToFile(imagesFromThread);
            if (writeWentWell)
                Console.WriteLine("Wrote images from thread to file.");
            else
                await Console.Error.WriteLineAsync("Couldnt Write properly to file.");

            return writeWentWell;
        }

        private async Task<List<ImageDto>> GetAllImagesAsStreams(HtmlNode thread)
        {
            var images = new List<ImageDto>();
            var imageLinkNodes = thread.SelectNodes(XPathConstants.AllImageLinks);
            var imageLinks = imageLinkNodes.Select(x => "https:" + x.Attributes[1].Value).ToList();
            foreach (var imageLink in imageLinks)
            {
                var image = await _client.DownloadFile(imageLink);
                if (image == null) continue;
                var imageDto = new ImageDto(image, imageLink);
                images.Add(imageDto);
            }

            return images;
        }

        private static string GetThreadUrl(HtmlNode chanThread)
        {
            var chanThreadId = chanThread.Attributes[1].Value;
            var strippedChanThreadId = chanThreadId.Replace("t", "");
            return BaseUrl + "thread/" + strippedChanThreadId;
        }

        private async Task<List<ImageDto>> GetImagesFromThread(string threadUrl)
        {
            Console.WriteLine("Requesting: " + threadUrl);
            using (var threadResponse = await _client.GetAsyncWithNewUserAgent(threadUrl))
            {
                using (var threadContent = threadResponse.Content)
                {
                    if (threadResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Trying to parse thread content.");
                        var threadHtmlDocument = await HtmlHelpers.ParseContentToHtmlDocument(threadContent);
                        return await GetAllImagesAsStreams(threadHtmlDocument.DocumentNode);
                    }

                    Console.WriteLine("404, did not find thread.");
                }
            }

            return new List<ImageDto>();
        }
    }
}