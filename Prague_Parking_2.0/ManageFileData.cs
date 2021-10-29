using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Prague_Parking_2._0
{
    public static class ManageFileData
    {
        //private const string priceFilePath = @"../../../Textfiles/PriceList.txt";
        private const string ParkingList = @"../../../Textfiles/Parkinglist.json";

        public static List<ParkingSpot> ReadParkinglist()//returns file as a non-json list of objects //TODO: handle exceptions
        {
            string temp = File.ReadAllText(ParkingList);
            var tempList = JsonConvert.DeserializeObject<List<ParkingSpot>>(temp);
            return tempList;
        }
        public static void UpdateParkingList<T>(List<T> list) 
        {
            string parkingHouseString = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(ParkingList, parkingHouseString);
        }
        //public static List<string> GetPriceFromFile()
        //{
        //    List<string> priceList = File.ReadAllLines(priceFilePath).ToList();
        //    return priceList;
        //}
    }
}
