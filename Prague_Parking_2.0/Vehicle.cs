using System;

namespace Prague_Parking_2._0
{
    //[Serializable]
    public class Vehicle
    {
        private string _regNumber;
        private DateTime _checkIn;
        public string VehicleType { get; init; } //typen är INTE till för att definiera vilken typ som ska läggas till, utan har bara en vehicletype för att göra en klarare utskrift
        public int Size { get; init; }

        public Vehicle(string regNum)
        {
            RegNumber = regNum;
            CheckIn = DateTime.Now;
        }
        
        public string RegNumber
        {
            get => _regNumber;
            init => _regNumber = value;
        }
        public DateTime CheckIn
        {
            get => _checkIn;
            init => _checkIn = value;
        }

    }
}
