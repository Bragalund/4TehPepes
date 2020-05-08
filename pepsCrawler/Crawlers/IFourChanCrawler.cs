using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using pepsCrawler.Models;

namespace pepsCrawler
{
    public interface I4ChanCrawler
    {
        Task<List<ImageDto>> GetPepesFrom4Chan();
    }
}