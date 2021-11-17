using System;

namespace Prague_Parking_2._0
{
    public class Car : Vehicle
    {
        Configurations config = Configurations.ReadConfigFile();
        
        public Car(string regNumber) : base(regNumber)
        {
            VehicleType = "CAR";
            Size = config.CarSize;
            CheckIn = DateTime.Now;
        }
    }
}
