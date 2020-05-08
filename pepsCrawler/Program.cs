using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using pepsCrawler.Crawlers;

namespace pepsCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fourChanCrawler = new FourChanCrawler(new FourChanHttpClient());
            await fourChanCrawler.GetPepesFrom4Chan();
            
         
        }
    }
}