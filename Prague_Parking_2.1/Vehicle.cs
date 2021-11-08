using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
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

    }
}
