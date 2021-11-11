using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Prague_Parking_2._1
{
    public class PriceConfiguration
    {
        public static int CarPrice { get; set; }
        public static int McPrice { get; set; }
        public static int BikePrice { get; set; }
        public static int BusPrice { get; set; }
        public static int FreeMinutes { get; set; }

        private const string PriceFilePath = @"../../../Datafiles/PriceList.txt";
        private const string PricingPath = @"../../../Datafiles/Prices.json";

        public static PriceConfiguration ReadPriceConfig()
        {
            string json = File.ReadAllText(PricingPath);
            var data = JsonConvert.DeserializeObject<PriceConfiguration>(json);
            return data;
        }
        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
