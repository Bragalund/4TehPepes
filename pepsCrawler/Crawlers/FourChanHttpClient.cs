using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using pepsCrawler.Helpers;

namespace pepsCrawler.Crawlers
{
    public class FourChanHttpClient
    {
        private HttpClient _httpClient;
        private Random _random;
        private List<string> _browsers;
        
        public FourChanHttpClient()
        {
            _random = new Random();
            _browsers = new List<string>()
            {
                // "Mozilla/5.0 CK={} (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
                "Mozilla/5.0 (iPad; CPU OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148",
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322)",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Mobile/15E148 Safari/604.1",
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)",
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36",
                "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16D57"
            };

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept",
                "text/html, application/xhtml+xml, application/xml;q=0.9, */*;q=0.8");
            _httpClient.DefaultRequestHeaders.Add("Accept-Charset", "utf-8, iso-8859-1;q=0.5, *;q=0.1");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", GetRandomUserAgent());
        }

        private string GetRandomUserAgent()
        {
            int randomIndexNumber = _random.Next(0, _browsers.Count - 1);
            return _browsers[randomIndexNumber];
        }

        public async Task<HttpResponseMessage> GetAsyncWithNewUserAgent(string requestUri)
        {
            var newUserAgent = SetNewUserAgent();
            Console.WriteLine("Changed to new UserAgent: " + newUserAgent);
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<Stream> DownloadFile(string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                var imageFormat = HtmlHelpers.GetImageFormatFromLink(link);
                if (Equals(imageFormat, ImageFormat.Jpeg) || Equals(imageFormat, ImageFormat.Png))
                {
                    SetNewUserAgent();
                    return await _httpClient.GetStreamAsync(link);
                }

                Console.WriteLine("Only supports jpeg and png. Not downloading image: " + link);
            }

            Console.WriteLine("Link was null or empty. Cannot download image.");
            return null;
        }

        private string SetNewUserAgent()
        {
            var currentUserAgent = _httpClient.DefaultRequestHeaders.UserAgent.ToString();
            var newUserAgentSet = "";
            var noNewUserAgent = true;
            while (noNewUserAgent == true)
            {
                var randomUserAgent = GetRandomUserAgent();
                if (randomUserAgent.Equals(currentUserAgent))
                {
                    // Should not change to same User-Agent
                }
                else
                {
                    noNewUserAgent = false;
                    _httpClient.DefaultRequestHeaders.Remove("User-Agent");
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", randomUserAgent);
                    newUserAgentSet = randomUserAgent;
                }
            }

            return newUserAgentSet;
        }
    }
}