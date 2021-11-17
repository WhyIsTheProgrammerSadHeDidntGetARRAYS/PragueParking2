using System;
using System.IO;

namespace Prague_Parking_2._0
{
    public class MC : Vehicle
    {
        Configurations config = Configurations.ReadConfigFile();
        
        public MC(string regNumber) : base(regNumber)
        {
            VehicleType = "MOTORCYCLE";
            Size = config.McSize;
        }
    }
}
