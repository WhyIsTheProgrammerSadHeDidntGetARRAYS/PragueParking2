using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Prague_Parking_2._0
{
    public static class Configurations
    {
        public static int ParkingHouseSize { get; set; }
        public static int ParkingSpotSize { get; set; }
        public static int CarCost { get; set; }
        public static int Mccost { get; set; }

        public const string ConfigFilePath = @"../../../Textfiles/Config.json";
        public const string PriceFilePath = @"../../../Textfiles/PriceList.txt";

        public static void SetConfigValues()
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(ConfigFilePath));
            JToken getParkingSpotSize = jsonFile.SelectToken("ParkingspotSize");
            JToken getPHouseSize = jsonFile.SelectToken("ParkingSpots");
            ParkingSpotSize = (int)getParkingSpotSize;
            ParkingHouseSize = (int)getPHouseSize;
        }
        public static void SetPrices()
        {
            List<string> priceList = GetPriceList();
            foreach (var price in priceList)
            {
                string[] split = price.Split('=');
                if (split.Contains("car"))
                {
                    CarCost = int.Parse(split[1]);
                }
                if (split.Contains("motorcycle"))
                {
                    CarCost = int.Parse(split[1]);
                }
            }
        }
        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
