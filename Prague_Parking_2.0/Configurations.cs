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
    public class Configurations
    {
        public int ParkingSpots { get; set; }
        public int ParkingSpotSize { get; set; }
        public int CarSize { get; set; }
        public int McSize { get; set; }
        public int CarPrice { get; set; }
        public int McPrice { get; set; }
        public int FreeMinutes { get; set; }

        private const string ConfigFilePath = @"../../../Textfiles/Config.json";
        private const string PriceFilePath = @"../../../Textfiles/PriceList.txt";

        public static Configurations ReadConfigFile()
        {
            string json = File.ReadAllText(ConfigFilePath);
            var data = JsonConvert.DeserializeObject<Configurations>(json);
            return data;
            
        }
        public static List<string> GetPriceList()
        {
            List<string> priceList = File.ReadAllLines(PriceFilePath).ToList();
            return priceList;
        }
    }
}
