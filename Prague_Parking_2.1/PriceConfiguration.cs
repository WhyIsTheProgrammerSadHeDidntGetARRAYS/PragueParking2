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
        public int CarPricePerHour { get; set; }
        public int MCPricePerHour { get; set; }
        public int BikePricePerHour { get; set; }
        public int BusPricePerHour { get; set; }
        public int FreeParkingTimeInMinutes { get; set; }

        private const string PriceFilePath = @"../../../Datafiles/PriceList.txt";
        private const string PricingPath = @"../../../Datafiles/Prices.json";


        public static PriceConfiguration ReadPriceConfig()
        {
            if (!File.Exists(PricingPath))
            {
                throw new FileNotFoundException("The file could not be found"); //this is bad if you dont handle the errors
            }
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
