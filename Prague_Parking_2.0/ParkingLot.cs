using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

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
            for (int i = 0; i < 100; i++) //kanske lite fult att hårdkoda 100, borde kanske läsa in settings från en fil som säger att storleken på parkeringshuset ska vara 100
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1, Space = 4, AvailableSpace = 4 });
            }
        }
        public ParkingSpot FirstAvailableSlot(Vehicle vehicle)
        {
            ParkingSpot spot = ParkingList.Find(x => x.AvailableSpace >= vehicle.Size); //hittar första plats där fordonet får plats. Funkar för både MC och Bil
            if (spot != null)
            {
                return spot;
            }
            return null;

        }
        public void AddCar() //temporär metod för att lägga till bil
        {
            Console.WriteLine("Type in registrationnumber for your Car");
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

        public void Search() //temporär sökmetod
        {
            Console.WriteLine("Type in registration number to search for");
            string regNumber = Console.ReadLine();

            var search =
                from p in ParkingList   
                from v in p.VehiclesParked  
                where v.RegNr == regNumber
                select v;

            foreach (var item in search)
            {
                Console.WriteLine("Vehicle {0} was found", item.VehicleType + " " + item.RegNr); //kanske ska skaffa en override string metod i vehicle, elr derived classes så man slipper loopa såhär för att skriva ut
            }

        }
        private List<Vehicle> GetParkedVehicles()
        {
            var list = new List<Vehicle>();
            foreach (var parkingspots in ParkingList)
            {
                foreach (var v in parkingspots.VehiclesParked)
                {
                    if(v != null)
                    {
                        list.Add(v);
                    }
                }
            }
            return list;
        }
        public void PrintVehicles()
        {
            Console.CursorVisible = false;
            var table = new Table();
            table.AddColumn(new TableColumn("[aqua]Parkinghouse overview[/]").Centered()).Alignment(Justify.Center);
            table.AddRow("[aqua]*NOTE* Each parkingspot is colormarked. The color of each parkingspot reflects the availability[/]");
            AnsiConsole.Write(table);

            Table t1 = new Table();
            t1.AddColumns("[grey]AVAILABLE SPOT =[/] [green]GREEN[/]", "[grey]FULL SPOT =[/] [red]RED[/]").Centered().Alignment(Justify.Center);
            AnsiConsole.Write(t1);
            
            for (int i = 0; i < 1; i++)
            {
                Table newTable = new Table().Centered();
                var parkingSpotColorMarking = "";
                var printResult = "";
                 
                for (int j = i; j < 100; j++) //fixa detta så att det läser parkinghousesize från en fil
                {
                    if (j >= 100) //fixa detta så att det läser parkinghousesize från en fil
                    {
                        break;
                    }
                    if (ParkingList[j].AvailableSpace == 4)
                    {
                        parkingSpotColorMarking = "green";
                    }
                    else
                    {
                        parkingSpotColorMarking = "red";
                    }
                    printResult += ($"[{parkingSpotColorMarking}] {j + 1}[/] ");
                }
                newTable.AddColumn(new TableColumn(printResult));
                //i += 24;
                AnsiConsole.Write(newTable);
                var haha = new Table();
                haha.AddColumn(new TableColumn("[grey] Press any key to get back to the menu[/]").Centered()).Alignment(Justify.Center);
                AnsiConsole.Write(haha);
                Console.ReadKey();
                Console.CursorVisible = true;
            }
        }
        public void PrintPrice()
        {
            Console.CursorVisible = false;
            List<string> prices = Configurations.GetPriceFromFile();
            var table = new Table();
            table.Expand();
            table.AddColumn(new TableColumn(new Markup("[green]Our prices[/]")).Alignment(Justify.Center));
            foreach (var item in prices)
            {
                table.AddRow(item);
            }
            AnsiConsole.Write(table);
            var haha = new Table();
            haha.AddColumn(new TableColumn("[grey] Press any key to get back to the menu[/]").Centered()).Alignment(Justify.Center);
            AnsiConsole.Write(haha);
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        //public void UpdateParkinglistFile() //har en egen metod för detta i configurations 
        //{
        //    string path = @"../../../Parkinglist.json";
        //    string parkingHouseString = JsonConvert.SerializeObject(ParkingList, Formatting.Indented);
        //    File.WriteAllText(path, parkingHouseString);
        //}
    }
}
