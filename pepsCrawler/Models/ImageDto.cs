using System.Drawing.Imaging;
using System.IO;
using pepsCrawler.Helpers;

namespace pepsCrawler.Models
{
    public class ImageDto
    {
        public ImageDto(Stream imageStream, string link, string threadLink)
        {
            ImageStream = imageStream;
            ChosenImageFormat = HtmlHelpers.GetImageFormatFromLink(link);
            ImageName = HtmlHelpers.GetImageNameFromLink(link);
            ThreadName = HtmlHelpers.GetThreadNumberFromLink(threadLink);
        }

        public Stream ImageStream { get; }
        public ImageFormat ChosenImageFormat { get; }
        public string ImageName { get; }
        public string ThreadName { get; }

        
    }
}