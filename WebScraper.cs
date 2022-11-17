using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;

namespace ElectricityPrice
{
    public class WebScraper
    {
        private readonly IMemoryCache _memoryCache;
        public  WebScraper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// GetS03Prices
        /// </summary>
        /// <param name="url">Url To Scrape</param>
        /// <returns></returns>
        public List<Double> GetS03Prices(string url)
        {
            List<Double> output;
            var cacheResult = _memoryCache.TryGetValue("S03", out output);
           
            if (cacheResult) { return output; }

            //Thread.Sleep(5000);
            List<Double> priceDataPoint = new List<Double>();
            HtmlWeb webSite = new HtmlWeb();
            HtmlDocument doc = webSite.Load(url);

            var nodes = doc.DocumentNode.SelectNodes("/html/body/div/div[8]/div/div/div/table/tbody/tr[position()>1]/td[2]");
            if (nodes == null || !nodes.Any()) return priceDataPoint;

            foreach (var node in nodes)
            {
                if (node == null || string.IsNullOrEmpty(node.InnerText)) continue;

                var onlyDigits = node.InnerText.ReplaceString(" öre/kWh", "", StringComparison.CurrentCulture);

                var gotADigit = Double.TryParse(onlyDigits, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out Double result);
                if (gotADigit)
                {
                    priceDataPoint.Add(result);
                }
            }

            _memoryCache.Set("S03", priceDataPoint, TimeSpan.FromHours(1));

            return priceDataPoint;


        }

    }
}
