using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Prague_Parking_2._1
{
    record ParkingHouseConfig(int ParkingspotSize, int ParkingSpots, int CarSize, int McSize, int BikeSize, int BusSize);
    record ParkingData(ParkingHouseConfig Configurations);
    record PriceConfig(int CarPricePerHour, int MCPricePerHour);
    record PriceData(PriceConfig Prices);
    public static class Configuration
    {
        public static int ParkingHouseSize { get; set; }
        public static int ParkingSpotSize { get; set; }
        public static int CarSize { get; set; }
        public static int McSize { get; set; }
        public static int BikeSize { get; set; }
        public static int BusSize { get; set; }
        public static int CarPrice { get; set; }
        public static int McPrice { get; set; }
        public static int FreeMinutes { get; set; }

        private const string ConfigFilePath = @"../../../Datafiles/config.json";
        private const string PriceFilePath = @"../../../Datafiles/PriceList.txt";
        private const string PricingPath = @"../../../Datafiles/Prices.json";

        public static void SetConfigValues()
        {
            string json = File.ReadAllText(ConfigFilePath);
            var data = JsonConvert.DeserializeObject<ParkingData>(json);
            
            ParkingHouseSize = data.Configurations.ParkingSpots;
            ParkingSpotSize = data.Configurations.ParkingspotSize;
            CarSize = data.Configurations.CarSize;
            McSize = data.Configurations.McSize;
            BikeSize = data.Configurations.BikeSize;
            BusSize = data.Configurations.BusSize;
        }
        public static void SetPrices()
        {
            string json = File.ReadAllText(PricingPath);
            var data = JsonConvert.DeserializeObject<PriceData>(json);
            CarPrice = data.Prices.CarPricePerHour;
            McPrice = data.Prices.MCPricePerHour;
        }
        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
