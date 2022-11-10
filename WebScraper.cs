using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ElectricityPrice
{
    public class WebScraper 
    {
        /// <summary>
        /// GetS03Prices
        /// </summary>
        /// <param name="url">Url To Scrape</param>
        /// <returns></returns>
        public List<Double> GetS03Prices(string url)
        {
            List<Double> priceDataPoint = new List<Double>();
            HtmlWeb webSite = new HtmlWeb();
            HtmlDocument doc = webSite.Load(url);

            var nodes = doc.DocumentNode.SelectNodes("/html/body/div/div[8]/div/div/div/table/tbody/tr[position()>1]/td[2]");
            if (nodes == null || !nodes.Any())  return priceDataPoint;
            
            foreach (var node in nodes)
            {
                if(node == null || string.IsNullOrEmpty(node.InnerText)) continue;

             
               var onlyDigits = node.InnerText.ReplaceString(" öre/kWh", "", StringComparison.CurrentCulture);
                var gotADigit = Double.TryParse(onlyDigits,   out Double result);
                if (gotADigit)
                {
                    priceDataPoint.Add(result);
                }
            }

            return priceDataPoint;

        

           
        }
       
    }
}
