using System;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pepsCrawler.Helpers
{
    public static class HtmlHelpers
    {
        public static async Task<HtmlDocument> ParseContentToHtmlDocument(HttpContent httpContent)
        {
            var result = await httpContent.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(result);
            return document;
        }
        
        public static ImageFormat GetImageFormatFromLink(string link)
        {
            var wordsSeperatedByPunctuation = link.Split(".");
            var lastWord = wordsSeperatedByPunctuation[wordsSeperatedByPunctuation.Length - 1];
            return lastWord switch
            {
                "jpg" => ImageFormat.Jpeg,
                "png" => ImageFormat.Png,
                _ => ImageFormat.Wmf
            };
        }

        public static string GetImageNameFromLink(string link)
        {
            var arrayOfStrings = link.Split("/");
            return arrayOfStrings[arrayOfStrings.Length - 1];
        }

        public static string GetThreadNumberFromLink(string link)
        {
            var arrayOfStrings = link.Split("/");
            return arrayOfStrings[arrayOfStrings.Length - 1];
        }
    }
}