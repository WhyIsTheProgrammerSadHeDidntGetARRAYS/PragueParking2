﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
    public class Bike : Vehicle
    {
        
        public Bike(string regNumber): base(regNumber)
        {
            VehicleType = "BIKE";
            ParkingConfiguration config = ParkingConfiguration.ReadParkingConfig();
            Size = config.BikeSize;
            CheckIn = DateTime.Now;
        }
    }
}
