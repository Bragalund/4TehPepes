using System;

namespace pepsCrawler.Helpers
{
    public static class StringConstants
    {
        // XPATH
        public const string LinksToThreadsOnMainPage = "//span[a='Click here']/a[@class='replylink']/@href";
        public const string AllThreadsOnMainPage = "//div[@class='thread']";
        public const string AllImageLinks = "//a[@class='fileThumb']/@href";
        
        // Place to save images
        public const string PathForSavingImages = "D:\\Peps\\DownloadedWithScript";
    }
}