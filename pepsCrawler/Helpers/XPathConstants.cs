using System;

namespace pepsCrawler.Helpers
{
    public static class XPathConstants
    {
        public const string LinksToThreadsOnMainPage = "//span[a='Click here']/a[@class='replylink']/@href";
        public const string AllThreadsOnMainPage = "//div[@class='thread']";
        public const string AllImageLinks = "//a[@class='fileThumb']/@href";
    }
}