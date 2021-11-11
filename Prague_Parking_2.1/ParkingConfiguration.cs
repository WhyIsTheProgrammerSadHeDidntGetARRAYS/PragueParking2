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
        public int ParkingSpotSize { get; set; }
        public int ParkingSpots { get; set; }
        public int CarSize { get; set; }
        public int McSize { get; set; }
        public int BikeSize { get; set; }
        public int BusSize { get; set; }

        private const string ConfigFilePath = @"../../../Datafiles/config.json";

        public static ParkingConfiguration ReadParkingConfig()
        {
            string json = File.ReadAllText(ConfigFilePath);
            var data = JsonConvert.DeserializeObject<ParkingConfiguration>(json);
            return data;
        }
    }
}
