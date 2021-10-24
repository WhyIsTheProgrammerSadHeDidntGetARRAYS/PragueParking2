using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prague_Parking_2._0
{
    public class ParkingLot : ParkingSpot
    {
        List<ParkingSpot> ParkingList { get; set; } = Configurations.ReadParkinglist(); //we TRY to set the list equal to objects of parkingspots from parkingfile

        public ParkingLot()
        {
            if (ParkingList == null)//if list is empty, we create a new list empty list, and "draw" a new parkinglot
            {
                ParkingList = new List<ParkingSpot>(100);
                AddNewParkinglot();
            }
        }
        private void AddNewParkinglot()
        {
            for (int i = 0; i < 100; i++) //kanske lite fult att hårdkoda 100, borde kanske på nått sätt läsa in settings från en fil som säger att storleken på parkeringshuset ska vara 100
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1, Space = 4, AvailableSpace = 4 });
            }
        }
        public ParkingSpot FirstAvailableSlot(Vehicle vehicle)
        {
            ParkingSpot spot = ParkingList.Find(x => x.AvailableSpace >= vehicle.Size); //hittar första plats där fordonet får plats
            if (spot != null)
            {
                return spot;
            }
            return null;

        }
        public void AddCar() //temporär metod för att lägga till bil(funkar bra)
        {

            Console.WriteLine("Add car");
            string regnum = Console.ReadLine();
            Car newCar = new Car(regnum);
            ParkingSpot ps = FirstAvailableSlot(newCar);
            if (ps != null)
            {
                ps.AddVehicle(newCar);
                //UpdateParkinglistFile();
                Configurations.UpdateParkingList(ParkingList); //denna metoden la jag i en annan fil för att inte ha sökvägen copypastat överallt. Gör precis som metoden UpdateParkinglistFile nedan, fast tar in en generisklista
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                Console.ReadKey();
            }
        }

        public void Search() //temporär sökmetod(funkar bra)
        {
            Console.WriteLine("Type in registration number to search for");
            string regNumber = Console.ReadLine();

            var search =
                from p in ParkingList   //söker igenom listan parkinglist
                from v in p.VehiclesParked  //söker igenom egenskapen/listan vehiclesparked
                where v.RegNr == regNumber
                select v;

            foreach (var item in search)
            {
                Console.WriteLine("Vehicle {0} was found", item.RegNr); //kanske ska skaffa en override string metod i vehicle, elr derived classes så man slipper loopa såhär för att skriva ut
            }

        }
        public void UpdateParkinglistFile() //har en egen metod för detta i configurations för att inte pasta sökvägar överallt
        {
            string path = @"../../../Parkinglist.json";
            string parkingHouseString = JsonConvert.SerializeObject(ParkingList, Formatting.Indented);
            File.WriteAllText(path, parkingHouseString);
        }
    }
}
