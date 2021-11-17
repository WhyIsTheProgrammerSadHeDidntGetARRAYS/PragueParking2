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
    public class ParkingConfiguration
    {
        public int ParkingSpotSize { get; init; }
        public int ParkingSpots { get; init; }
        public int CarSize { get; init; }
        public int McSize { get; init; }
        public int BikeSize { get; init; }
        public int BusSize { get; init; }

        private const string ConfigFilePath = @"../../../Datafiles/config.json";

        public static ParkingConfiguration ReadParkingConfig() //konstruktorn kan ropa på denna, ta bort static
        {
            if (!File.Exists(ConfigFilePath))
            {
                throw new FileNotFoundException("The file 'Datafiles/config.json' could not be found"); 
            }
            string json = File.ReadAllText(ConfigFilePath);
            var data = JsonConvert.DeserializeObject<ParkingConfiguration>(json);
            return data;
        }
        public void WriteToParkingConfig(int parkingspots)
        {
            if (!File.Exists(ConfigFilePath))
            {
                throw new FileNotFoundException("The file 'Datafiles/config.json' could not be found");
            }
            string json = File.ReadAllText(ConfigFilePath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj["ParkingSpots"] = parkingspots;
            string jsonConvert = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, jsonConvert);
        }
    }
}
