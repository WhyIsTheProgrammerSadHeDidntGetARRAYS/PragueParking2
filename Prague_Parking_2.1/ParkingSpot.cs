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
        public int ParkingSpotSize { get; } 
        public int AvailableSpace { get; set; }
        public List<Vehicle> VehiclesParked { get; set; } = new List<Vehicle>();

        public ParkingSpot()
        {
            ParkingConfiguration config = ParkingConfiguration.ReadParkingConfig();
            ParkingSpotSize = config.ParkingSpotSize;
            AvailableSpace = config.ParkingSpotSize;
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
        //lägg till metod för att hantera större fordon
    }
}
