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
        private const string priceFilePath = @"../../../PriceList.txt";
        private const string ParkingList = @"../../../Parkinglist.json";
        private const string ConfigFile = @"../../../Config.json";

        public static List<ParkingSpot> ReadParkinglist()//returns a list of parkinglist and parklinglist properties(parkinghouse file)
        {
            string temp = File.ReadAllText(ParkingList);
            var tempList = JsonConvert.DeserializeObject<List<ParkingSpot>>(temp);
            return tempList;
        }
        public static void UpdateParkingList<T>(List<T> list) //generic type för att slippa referera till parkingspots klassen...
        {
            string temp = File.ReadAllText(ParkingList);
            string parkingHouseString = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(ParkingList, parkingHouseString);
        }
        //public static void SetConfigValues()
        //{
        //    dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(ConfigFile));
        //    JToken setPHouseSize = jsonFile.SelectToken("ParkingSpots");
        //}
        public static List<string> GetPriceFromFile()
        {
            List<string> priceList = File.ReadAllLines(priceFilePath).ToList();
            return priceList;
        }
    }
}
