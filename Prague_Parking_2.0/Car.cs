using System;

namespace Prague_Parking_2._0
{
    public class Car : Vehicle
    {
        //public Car() // för ett test..
        //{
        //    Size = 4;
        //}
        public Car(string regNumber)
        {
            VehicleType = "CAR";
            RegNr = regNumber;
            Size = 4;
            CheckIn = DateTime.Now;
        }

        public override string ToString()
        {
            return "Regnummer: " + RegNr;
        }
    }
}
