using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace Prague_Parking_2._0
{
    public class ParkingLot 
    {
        List<ParkingSpot> ParkingList { get; set; } = ManageFileData.ReadParkinglist(); //we TRY to set the list equal to objects of parkingspots from parkingfile

        public ParkingLot()
        {
            if (ParkingList == null)//if list is empty, we create a new list empty list, and "draw" a new parkinglot
            {
                ParkingList = new List<ParkingSpot>(capacity: 100);
                AddNewParkinglot();
            }
        }
        
        public ParkingSpot FirstAvailableSlot(Vehicle vehicle) //kommer fungera för bil, mc och cykel, men inte buss
        {
            ParkingSpot spot = ParkingList.Find(x => x.AvailableSpace >= vehicle.Size); //hittar första plats där fordonet får plats. 
            if (spot != null)
            {
                return spot;
            }
            return null;
        }
        public void AddCar() //temporär metod för att testa lägga till bil
        {
            Console.WriteLine("Type in registrationnumber for your Car");
            string regnum = Console.ReadLine();
            Car newCar = new Car(regnum);
            ParkingSpot ps = FirstAvailableSlot(newCar);
            if (ps != null)
            {
                ps.AddVehicle(newCar);
                ManageFileData.UpdateParkingList(ParkingList); 
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                Console.ReadKey();
            }
        }
        public void AddMC() //temporär metod för att testa lägga till MC
        {
            Console.WriteLine("Type in registrationnumber for your Motorcycle");
            string regnum = Console.ReadLine();
            MC newCar = new MC(regnum);
            ParkingSpot ps = FirstAvailableSlot(newCar);
            if (ps != null)
            {
                ps.AddVehicle(newCar);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                Console.ReadKey();
            }
        }
        public void Remove() //temporär. Ska ju kombineras med sökmetoden
        {
            Console.WriteLine("Type in registrationnumber of the vehicle you'd like to remove");
            string regnum = Console.ReadLine();

            //var search =
            //    from p in ParkingList
            //    from v in p.VehiclesParked
            //    where v.RegNr == regnum
            //    select v;
            foreach (var spot in ParkingList)
            {
                foreach (var vehicle in spot.VehiclesParked)
                {
                    if (vehicle.RegNr == regnum)
                    {
                        spot.RemoveVehicle(vehicle);
                        ManageFileData.UpdateParkingList(ParkingList);
                        break;
                    }
                }
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
                Console.WriteLine("It's a match. Vehicle was found!\nVehicleinfo: {0}, RegNr: {1}", item.VehicleType, item.RegNr); //kanske ska skaffa en override string metod i vehicle, elr derived classes så man slipper loopa såhär för att skriva ut
            }

        }
        private List<Vehicle> GetParkedVehicles()
        {
            var list = new List<Vehicle>();
            foreach (var parkingspots in ParkingList)
            {
                foreach (var v in parkingspots.VehiclesParked)
                {
                    if (v != null)
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
            t1.AddColumns("[grey]EMPTY SPOT =[/] [green]GREEN[/]", "[grey]FULL SPOT =[/] [red]RED[/]").Centered().Alignment(Justify.Center);
            AnsiConsole.Write(t1);

            Table newTable = new Table().Centered();
            var parkingSpotColorMarking = "";
            var printResult = "";

            for (int i = 0; i < Configurations.ParkingHouseSize; i++) //fixa detta så att det läser parkinghousesize från en fil
            {
                if (ParkingList[i].AvailableSpace == 4)
                {
                    parkingSpotColorMarking = "green";
                }
                else
                {
                    parkingSpotColorMarking = "red";
                }
                printResult += ($"[{parkingSpotColorMarking}] {i + 1}[/] ");
            }
            newTable.AddColumn(new TableColumn(printResult));
            AnsiConsole.Write(newTable);
            var backToMenu = new Table();
            backToMenu.AddColumn(new TableColumn("[grey] Press any key to get back to the menu[/]").Centered()).Alignment(Justify.Center);
            AnsiConsole.Write(backToMenu);
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        public void PrintPrice()
        {
            Console.CursorVisible = false;
            List<string> prices = Configurations.GetPriceList();
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
        private void AddNewParkinglot()
        {
            //int parkingHouseSize = config.SetConfigValues();
            for (int i = 0; i < Configurations.ParkingHouseSize; i++) //läs in settings från en fil som säger att storleken på parkeringshuset ska vara 100
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1/*, AvailableSpace = 4*/ });
            }
        }
    }
}
