using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
    public class Car : Vehicle
    {
        ParkingConfiguration config = ParkingConfiguration.ReadParkingConfig();

        public Car(string regNumber) : base(regNumber)
        {
            VehicleIdentifier = "CAR";
            Size = config.CarSize;
        }
    }
}
