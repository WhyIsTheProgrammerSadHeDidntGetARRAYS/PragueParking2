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
        public void WriteToPriceConfig(string option, int newPrice)
        {
            if (!File.Exists(PricingPath))
            {
                throw new FileNotFoundException("The file 'Datafiles/config.json' could not be found");
            }
            string json = File.ReadAllText(PricingPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[option] = newPrice;
            string jsonConvert = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(PricingPath, jsonConvert);
        }

        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
