﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._0
{
    public class ParkingSpot
    {
        public int ParkingWindow { get; set; }
        public int ParkingSpotSize { get; } //behöver ingen setter just nu, då space per ruta alltid är 4
        public int AvailableSpace { get; set; }
        public List<Vehicle> VehiclesParked { get; set; } = new List<Vehicle>(); 

        public ParkingSpot()
        {
            Configurations config = Configurations.ReadConfigFile();
            ParkingSpotSize = config.ParkingSpotSize;
            AvailableSpace = config.ParkingSpotSize;
        }

        public bool AddVehicle(Vehicle vehicle)//lägga till ett fordon
        {
            VehiclesParked.Add(vehicle);
            AvailableSpace -= vehicle.Size;
            return true;
        }

        public bool RemoveVehicle(Vehicle vehicle)//ta bort ett fordon(endast detta kan nog bli lite skevt ifall man ska lägga till/ta bort en buss(16units)..)
        {
            VehiclesParked.Remove(vehicle);
            AvailableSpace += vehicle.Size;
            return true;
        }
    }
}
