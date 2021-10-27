using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Prague_Parking_2._0
{
    class Program
    {
        private const string ConfigFile = @"../../../Config.json"; //temporärt för ett test
        static void Main(string[] args)
        {
            //GetConfigValues();
            //Console.ReadKey();
            Menu menu = new Menu();
            while (true)
            {
                menu.MenuChoices();
            }
        }

        public static void GetConfigValues() //temporärt för ett test
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(ConfigFile));
            JToken setPHouseSize = jsonFile.SelectToken("ParkingSpots");
            Console.WriteLine($"Amount of parkingspots: {setPHouseSize}");

            JToken setParkingSpotSize = jsonFile.SelectToken("ParkingspotSize");
            Console.WriteLine($"Amount of space in one parkingwindow: {setParkingSpotSize}");
        }
    }
}
