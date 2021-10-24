using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Prague_Parking_2._0
{
    public static class Configurations
    {
        public const string priceFilePath = @"../../../PriceList.txt";
        public const string ParkingList = @"../../../Parkinglist.json";

        public static List<ParkingSpot> ReadParkinglist()//returns a list of parkinglist and parklinglist properties(eg. parkinghouse file)
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

        public static void GetPriceFromFile()
        {
            Console.WriteLine("Our prices:\n");
            List<string> pricelist = File.ReadAllLines(priceFilePath).ToList();
            foreach (var price in pricelist)
            {
                Console.WriteLine(price);
            }
        }
    }
}
