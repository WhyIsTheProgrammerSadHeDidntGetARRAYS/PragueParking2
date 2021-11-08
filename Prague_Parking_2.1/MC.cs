using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
    public class MC : Vehicle
    {
        public MC(string regNumber) : base(regNumber)
        {
            VehicleType = "MOTORCYCLE";
            //RegNr = regNumber;
            Size = Configuration.McSize;
            CheckIn = DateTime.Now;

        }
    }
}
