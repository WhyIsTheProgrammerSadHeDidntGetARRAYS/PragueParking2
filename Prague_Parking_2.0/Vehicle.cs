using System;

namespace Prague_Parking_2._0
{
    //[Serializable]
    public class Vehicle
    {
        public string RegNumber { get; init; }
        public string VehicleType { get; set; } //typen är INTE till för att definiera vilken typ som ska läggas till, utan har bara en vehicletype för att göra en klarare utskrift
        public DateTime CheckIn { get; set; }
        public int Size { get; set; }

        public Vehicle(string regNum)
        {
            RegNumber = regNum;
        }
        
        //public override string ToString()
        //{
        //    return "Registration number: " + regNum;
        //}

        public string CheckOut(Vehicle vehicle)
        {
            DateTime checkOut = DateTime.Now;
            TimeSpan parkedTime = checkOut - vehicle.CheckIn;

            return parkedTime.ToString();

        }

    }
}
