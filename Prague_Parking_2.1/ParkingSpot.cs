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
        ParkingConfiguration config = ParkingConfiguration.ReadParkingConfig();

        public ParkingSpot()
        {
            ParkingSpotSize = config.ParkingSpotSize;
            AvailableSpace = config.ParkingSpotSize;
        }
        public void AddVehicle(Vehicle vehicle)
        {
            VehiclesParked.Add(vehicle);
            
            if(vehicle.Size > ParkingSpotSize) //hanterar availablespace på större fordon, och för buss tar den 1/4 storlek av bussen & ställer per ruta
            {
                AvailableSpace -= vehicle.Size / config.ParkingSpotSize;
            }
            else //är fordonets storlek mindre än / lika med parkeringsrutans totala storlek, fungerar det att dra av fordonets storlek från rutans tillgängliga size
            {
                AvailableSpace -= vehicle.Size;
            }
        }
        public void RemoveVehicle(Vehicle vehicle)
        {
            VehiclesParked.Remove(vehicle);

            if(vehicle.Size > ParkingSpotSize)
            {
                AvailableSpace += vehicle.Size / config.ParkingSpotSize;
            }
            else
            {
                AvailableSpace += vehicle.Size;
            }
        }
        //public void AddSmallVehicle(Vehicle vehicle)//l(Gjorde om dessa metoder till de ovan, då allt går att hanteras i 2 metoder)
        //{
        //    VehiclesParked.Add(vehicle);
        //    AvailableSpace -= vehicle.Size;
        //}

        //public void RemoveSmallVehicle(Vehicle vehicle)
        //{
        //    VehiclesParked.Remove(vehicle);
        //    AvailableSpace += vehicle.Size;
            
        //}
        
        //public void AddBigVehicle(Vehicle vehicle) // bussen tar en 1/4 av sin storlek per ruta
        //{
        //    VehiclesParked.Add(vehicle);
        //    AvailableSpace -= vehicle.Size / config.ParkingSpotSize;
        //}
        //public void RemoveBigVehicle(Vehicle vehicle)
        //{
        //    VehiclesParked.Remove(vehicle);
        //    AvailableSpace += vehicle.Size / config.ParkingSpotSize;
        //}

        public override string ToString() //en override strängmetod för att rutan ska "skriva ut sig själv med parkeringsnummer"
        {
            return $"Spot:{ParkingWindow}";
        }
    }
}
