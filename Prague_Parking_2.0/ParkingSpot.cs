using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._0
{
    public class ParkingSpot
    {
        public int ParkingWindow { get; set; }
        public int Space { get; set; }
        public int AvailableSpace { get; set; }
        public List<Vehicle> VehiclesParked { get; set; } = new List<Vehicle>(); //lite fult kanske, men ger mig en lista som en property, där jag kan lägga till objekt på respektive parkeringsplats

        public ParkingSpot()
        {

        }

        public void AddVehicle(Vehicle vehicle)//lägga till ett fordon
        {
            VehiclesParked.Add(vehicle);
            AvailableSpace -= vehicle.Size;
        }

        public void RemoveVehicle(Vehicle vehicle)//ta bort ett fordon(endast detta kan nog bli lite skevt ifall man ska lägga till/ta bort en buss(16units)..)
        {
            VehiclesParked.Remove(vehicle);
            AvailableSpace += vehicle.Size;
        }

        public override string ToString()
        {
            return "";
        }

    }
}
