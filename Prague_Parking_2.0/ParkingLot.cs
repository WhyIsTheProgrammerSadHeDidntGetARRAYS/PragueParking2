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
            Car car = new Car(regnum);
            ParkingSpot ps = FirstAvailableSlot(car);
            if (ps != null)
            {
                ps.AddVehicle(car);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                Console.ReadKey();
            }
        }
        public void AddMC() //temporär metod för att testa lägga till MC
        {
            Console.WriteLine("Type in registrationnumber for your Motorcycle");
            string regnum = Console.ReadLine();
            MC mc = new MC(regnum);
            ParkingSpot ps = FirstAvailableSlot(mc);
            if (ps != null)
            {
                ps.AddVehicle(mc);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                Console.ReadKey();
            }
        }
        public void Remove() //temporär
        {
            Console.WriteLine("Give me the regnumber of vehicle to remove");
            string regNum = Console.ReadLine();
            (Vehicle veh, ParkingSpot spot) = GetVehicleInfo(regNum);
            
            if(veh != null)
            {
                spot.RemoveVehicle(veh);
                ManageFileData.UpdateParkingList(ParkingList);
            }
        }
        public void MoveVehicle() //jobbar på det D: borde nog skaffa en extra metod för att kolla ifall fordonet får plats i ÖNSKAD ruta
        {
            Console.WriteLine("Give me the regnumber of vehicle to move");
            string regNum = Console.ReadLine();
            (Vehicle veh, ParkingSpot spot) = GetVehicleInfo(regNum);

            Console.WriteLine("Type in your desired spot to move too [1- 100]");
            int parkingSpot;
            bool validation = int.TryParse(Console.ReadLine(), out parkingSpot) && veh != null;
            if (validation)
            {
                if(ParkingList[parkingSpot - 1].AvailableSpace >= veh.Size)
                {
                    ParkingList[parkingSpot - 1].AddVehicle(veh);
                    spot.RemoveVehicle(veh); // tar bort det från aktuella platsen
                    ManageFileData.UpdateParkingList(ParkingList);
                }
            }
        }
        private (Vehicle, ParkingSpot) GetVehicleInfo(string regNum)
        {
            var look =
                from p in ParkingList
                from v in p.VehiclesParked
                where v.RegNr == regNum
                select (v, p);

            foreach (var item in look)
            {
                if (item.v.RegNr == regNum)
                {
                    return (item.v, item.p);
                }
            }
            return (null, null);
        }
        public void SearchForVehicle()
        {
            Console.WriteLine("Type in registration number to search for");
            string regNumber = Console.ReadLine();
            var check = Search(regNumber);

            foreach (var item in check)
            {
                if (item.RegNr == regNumber)
                {
                    Console.WriteLine("It's a match. Vehicle was found!\nVehicleinfo: {0}, RegNr: {1}", item.VehicleType, item.RegNr);
                    return;
                }
            }
            Console.WriteLine("Vehicle not found!");
        }
        private IEnumerable<Vehicle> Search(string regNumber) //temporär sökmetod
        {
            var search =
                from p in ParkingList
                from v in p.VehiclesParked
                where v.RegNr == regNumber
                select v;

            return search;

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
