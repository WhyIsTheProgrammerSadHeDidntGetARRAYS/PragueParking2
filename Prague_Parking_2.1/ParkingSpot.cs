using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
    public class ParkingSpot
    {
        public int ParkingWindow { get; set; }
        public int ParkingSpotSize { get; } //behöver ingen setter just nu, då space per ruta alltid är 4
        public int AvailableSpace { get; set; }
        public List<Vehicle> VehiclesParked { get; set; } = new List<Vehicle>();

        public ParkingSpot()
        {
            ParkingSpotSize = Configuration.ParkingSpotSize;
            AvailableSpace = Configuration.ParkingSpotSize;
        }

        public bool AddVehicle(Vehicle vehicle)//lägga till ett fordon
        {
            VehiclesParked.Add(vehicle);
            AvailableSpace -= vehicle.Size;
            return true;
        }

        public void RemoveVehicle(Vehicle vehicle)//ta bort ett fordon
        {
            VehiclesParked.Remove(vehicle);
            AvailableSpace += vehicle.Size;
            //return true;
        }

        //public int TimeSpent(Vehicle vehicle) //
        //{
        //    TimeSpan timeParked = DateTime.Now - vehicle.CheckIn;
            
        //    int timeSpent = 0;

        //    if(timeParked.Minutes <= Configuration.FreeMinutes)
        //    {
        //        return timeSpent;
        //    }
        //    else if(timeParked.TotalMinutes >= 60)
        //    {
        //        timeSpent = (int)timeParked.TotalHours;
        //        return timeSpent;
        //    }
        //}
    }
}
