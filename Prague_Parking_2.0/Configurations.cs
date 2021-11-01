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
    record ParkingHouseConfig(int ParkingspotSize, int ParkingSpots);
    record ParkingData(ParkingHouseConfig Configurations);
    record PriceConfig(int CarPricePerHour, int MCPricePerHour);
    record PriceData(PriceConfig Prices);
    
    public static class Configurations
    {
        public static int ParkingHouseSize { get; set; }
        public static int ParkingSpotSize { get; set; }
        public static int CarCost { get; set; }
        public static int Mccost { get; set; }
        public static int FreeMinutes { get; set; }

        private const string ConfigFilePath = @"../../../Textfiles/Config.json";
        private const string PriceFilePath = @"../../../Textfiles/PriceList.txt";
        private const string Pricings = @"../../../Textfiles/Prices.json";

        public static void SetConfigValues()
        {
            string json = File.ReadAllText(ConfigFilePath);
            var data = JsonConvert.DeserializeObject<ParkingData>(json);
            ParkingHouseSize = data.Configurations.ParkingSpots;
            ParkingSpotSize = data.Configurations.ParkingspotSize;

        }
        public static void SetPrices()
        {
            string json = File.ReadAllText(Pricings);
            var data = JsonConvert.DeserializeObject<PriceData>(json);
            CarCost = data.Prices.CarPricePerHour;
            Mccost = data.Prices.MCPricePerHour;
        }
        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
