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
            var linkEnding = link.Substring(link.IndexOf(".", StringComparison.Ordinal) + 1);
            return linkEnding switch
            {
                "jpg" => ImageFormat.Jpeg,
                "png" => ImageFormat.Png,
                _ => ImageFormat.Jpeg
            };
        }

        public static string GetImageNameFromLink(string link)
        {
            var arrayOfStrings = link.Split("/");
            return arrayOfStrings[arrayOfStrings.Length - 1];
        }
    }
}