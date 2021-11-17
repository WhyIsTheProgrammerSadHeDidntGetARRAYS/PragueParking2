using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace Prague_Parking_2._0
{
    public class ParkingLot
    {
        List<ParkingSpot> ParkingList { get; set; } = ManageFileData.ReadParkinglist(); //we TRY to set the list equal to objects of parkingspots from parkingfile
        Configurations config = Configurations.ReadConfigFile();

        public ParkingLot()
        {
            if (ParkingList == null)//if list is empty, we create a new list empty list, and "draw" a new parkinglot
            {
                ParkingList = new List<ParkingSpot>(capacity: 100);
                AddNewParkinglot();
            }
        }
        /// <summary>
        /// Find the first available slot for a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public ParkingSpot FirstAvailableSlot(Vehicle vehicle)
        {
            ParkingSpot spot = ParkingList.Find(x => x.AvailableSpace >= vehicle.Size);
            return spot;
        }
        public void AddVehicle(Vehicle vehicle)
        {
            ParkingSpot ps = FirstAvailableSlot(vehicle);
            
            if(ps != null)
            {
                ps.AddVehicle(vehicle);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                UserDialogue.SuccessMessage("parked");
                Console.ReadKey();
            }
            else
                UserDialogue.ParkinglotFull();
                Console.ReadKey();
        }

        /// <summary>
        /// remove a vehicle
        /// </summary>
        public void CheckOutVehicle() //temporär
        {
            UserDialogue.DisplayOption("CHECKOUT VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            (Vehicle veh, ParkingSpot spot) = GetVehicleStatus(regnum);

            if (veh != null)
            {
                spot.RemoveVehicle(veh);
                ManageFileData.UpdateParkingList(ParkingList);
                UserDialogue.SuccessMessage("checked out");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// move vehicle to antoher spot
        /// </summary>
        public void MoveVehicle() 
        {
            UserDialogue.DisplayOption("MOVE VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            
            (Vehicle veh, ParkingSpot spot) = GetVehicleStatus(regnum);

            Console.WriteLine("Type in your desired spot to move too [1- 100]");
            int parkingSpot;
            bool validation = int.TryParse(Console.ReadLine(), out parkingSpot) && veh != null;
            if (validation)
            {
                if (ParkingList[parkingSpot - 1].AvailableSpace >= veh.Size)
                {
                    ParkingList[parkingSpot - 1].AddVehicle(veh); //lägger till på nya platsen
                    spot.RemoveVehicle(veh); // tar bort det från "gamla" platsen
                    ManageFileData.UpdateParkingList(ParkingList);
                    UserDialogue.SuccessMessage("moved");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("That spot is currently occupied!");
                    Console.ReadKey();
                }
            }
            UserDialogue.ErrorMessage();
        }

        /// <summary>
        /// returns a vehicle object AND where its standing in the parkinglot
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns></returns>
        private (Vehicle, ParkingSpot) GetVehicleStatus(string regNum)
        {
            var look =
                from p in ParkingList
                from v in p.VehiclesParked
                where v.RegNumber == regNum
                select (v, p);

            foreach (var item in look)
            {
                if (item.v.RegNumber == regNum)
                {
                    return (item.v, item.p);
                }
            }
            return (null, null);
        }
        public void SearchForVehicle()
        {
            UserDialogue.DisplayOption("SEARCH VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            var check = Search(regnum);

            foreach (var item in check)
            {
                if (item.RegNumber == regnum)
                {
                    Console.WriteLine("It's a match. Vehicle was found!\nVehicleinfo: {0}, Regnumber: {1}", item.VehicleType, item.RegNumber);
                    return;
                }
            }
            Console.WriteLine("Vehicle not found!");
        }
        /// <summary>
        /// searches for regnumber
        /// </summary>
        /// <param name="regNumber"></param>
        /// <returns>IEnumerable</returns>
        private IEnumerable<Vehicle> Search(string regNumber) //temporär sökmetod
        {
            var search =
                from p in ParkingList
                from v in p.VehiclesParked
                where v.RegNumber == regNumber
                select v;

            return search;

        }

        /// <summary>
        /// check if a regnumber is valid, meaning only capital letters, followed by numbers
        /// all within the range of 1-10
        /// </summary>
        /// <param name="regNr"></param>
        /// <returns></returns>
        public static bool IsValidRegNum(string regNr)
        {
            Regex input = new Regex("[A-Z][1-9]{1,10}");
            Match validation = input.Match(regNr);

            if (validation.Success)
                return true;

            return false;
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
        public void PrintParkedVehicles() //kolla på denna
        {
            var list = GetParkedVehicles();
            Console.CursorVisible = false;
            var table = new Table();
            table.AddColumn(new TableColumn("[grey]Parked Vehicles[/]").Centered()).Alignment(Justify.Center);
        }
        public void ParkingLotOverview()
        {
            UserDialogue.PrintParkingLotTable();

            Table newTable = new Table().Centered();
            var parkingSpotColorMarking = "";
            var printResult = "";

            for (int i = 0; i < config.ParkingSpots; i++) //fixa detta så att det läser parkinghousesize från en fil
            {
                if (ParkingList[i].AvailableSpace == config.ParkingSpotSize)
                {
                    parkingSpotColorMarking = "green";
                }
                else if (ParkingList[i].AvailableSpace == config.McSize)
                {
                    parkingSpotColorMarking = "yellow";
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
            var newTable = new Table();
            newTable.AddColumn(new TableColumn("[grey] Press any key to get back to the menu[/]").Centered()).Alignment(Justify.Center);
            AnsiConsole.Write(newTable);
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        private void AddNewParkinglot() //om det inte finns något i filen
        {
            for (int i = 0; i < config.ParkingSpots; i++) 
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });   
            }
        }
    }
}
