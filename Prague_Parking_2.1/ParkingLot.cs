using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace Prague_Parking_2._1
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
            else if (ParkingList.Count < Configuration.ParkingHouseSize)
            {
                ExpandParkingLot(); //jag kanske kan göra detta till ett menyval för skojs skull(att göra pshuet större/mindre)
            }
            ManageFileData.UpdateParkingList(ParkingList);
        }
        /// <summary>
        /// Find the first available slot for a small vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public ParkingSpot FindSmallVehicleSpot(Vehicle vehicle)
        {
            ParkingSpot spot = ParkingList.Find(x => x.AvailableSpace >= vehicle.Size);
            return spot;
        }
        /// <summary>
        /// using a hashset to store parkingspots with enough space in sequence, to park a bigger vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>A HashSet of parkingspots</returns>
        private HashSet<ParkingSpot> FindBigVehicleSpots(Vehicle vehicle)//teoretiskt sätt hade detta funkat med de små fordonen också
        {
            var set = new HashSet<ParkingSpot>();
            int counter = 0;
            int spotsInARow = vehicle.Size / Configuration.ParkingSpotSize;

            foreach (var spot in ParkingList)
            {
                if (spot.AvailableSpace == Configuration.ParkingSpotSize)
                {
                    counter++;
                    set.Add(spot);
                }
                else
                {
                    counter = 0;
                    set.Clear();
                }
                if (counter == spotsInARow)
                {
                    return set;
                }
            }
            return null;
        }

        /// <summary>
        /// Handles the parking of a big vehicles(bus). Parks it 4 times, because thats what it takes....
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bus"></param>
        public void ParkBigVehicle(Vehicle vehicle)
        {
            int resetAvailablespace = 0;
            var set = FindBigVehicleSpots(vehicle);

            if (set != null)
            {
                foreach (var item in set)
                {
                    item.AddVehicle(vehicle);
                    item.AvailableSpace = resetAvailablespace;
                }
                ManageFileData.UpdateParkingList(ParkingList);
                UserDialogue.SuccessMessage("parked");
                Console.ReadKey();
            }
            else
                UserDialogue.ParkinglotFull();

            Console.ReadKey();
        }

        /// <summary>
        /// handles parking of all "small vehicles"
        /// </summary>
        /// <param name="vehicle"></param>
        public void ParkSmallVehicle(Vehicle vehicle)
        {
            ParkingSpot spot = FindSmallVehicleSpot(vehicle);
            if (spot != null)
            {
                spot.AddVehicle(vehicle);
                ManageFileData.UpdateParkingList(ParkingList);
                UserDialogue.SuccessMessage("parked");
                Console.WriteLine("Your vehicle is standing at parkingwindow {0}", spot.ParkingWindow);
                Console.ReadKey();
                return;
            }
            else
                UserDialogue.ParkinglotFull();

            Console.ReadKey();
        }

        /// <summary>
        /// searches for vehicles with a certain regnumber
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns>Dictionary of KEY parkingspots and VALUE vehicles</returns>
        private Dictionary<ParkingSpot, Vehicle> GetSpecificVehicle(string regNum)
        {
            var dictionary = new Dictionary<ParkingSpot, Vehicle>();

            foreach (var spot in ParkingList)
            {
                foreach (var vehicle in spot.VehiclesParked)
                {
                    if (vehicle.RegNumber.Equals(regNum))
                    {
                        dictionary.Add(spot, vehicle);
                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// remove a vehicle
        /// </summary>
        public void RemoveVehicle()
        {
            UserDialogue.DisplayOption("CHECKOUT VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            Dictionary<ParkingSpot, Vehicle> set = GetSpecificVehicle(regnum);

            if (set.Count > 0)
            {
                foreach (var item in set)
                {
                    if (item.Value.VehicleType.Equals("BUS"))
                    {
                        item.Key.RemoveVehicle(item.Value);
                        item.Key.AvailableSpace = Configuration.ParkingSpotSize;
                    }
                    else
                    {
                        item.Key.RemoveVehicle(item.Value);
                    }

                }
                ManageFileData.UpdateParkingList(ParkingList);
                UserDialogue.SuccessMessage("checked out");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Vehicle not found!");
            Console.ReadKey();
        }
        /// <summary>
        /// move vehicle to antoher spot
        /// </summary>
        public void MoveVehicle() //jobbar på det D: 
        {
            UserDialogue.DisplayOption("MOVE VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            Dictionary<ParkingSpot, Vehicle> set = GetSpecificVehicle(regnum);

            Console.WriteLine("Type in your desired spot to move too [1- 100]");
            int parkingSpot;
            bool validation = int.TryParse(Console.ReadLine(), out parkingSpot) && set.Count > 0;
            if (validation)
            {
                parkingSpot -= 1; // to get the right index
                foreach (var item in set)
                {
                    if (item.Value.Size == Configuration.BusSize)
                    {
                        if (DoesBigVehicleFit(item.Value, parkingSpot))
                        {
                            ParkingList[parkingSpot].AddVehicle(item.Value);
                            ParkingList[parkingSpot++].AvailableSpace = 0;
                            item.Key.RemoveVehicle(item.Value);
                            item.Key.AvailableSpace = Configuration.ParkingSpotSize;
                        }
                        else
                        {
                            UserDialogue.ErrorMessage();
                            Console.ReadKey();
                            return;
                        }
                    }
                    else
                    {
                        if (DoesSmallVehicleFit(item.Value, parkingSpot))
                        {
                            ParkingList[parkingSpot].AddVehicle(item.Value); //lägger till på nya platsen
                            item.Key.RemoveVehicle(item.Value); // tar bort det från "gamla" platsen
                            ManageFileData.UpdateParkingList(ParkingList);
                            UserDialogue.SuccessMessage("moved");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            UserDialogue.ErrorMessage();
                            Console.ReadKey();
                            return;
                        }
                        
                    }
                }
            }
            UserDialogue.SuccessMessage("moved");
            ManageFileData.UpdateParkingList(ParkingList);
            Console.ReadKey();
        }
        /// <summary>
        /// checks if smaller vehicle fits in a given parkingspot from the user
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool DoesSmallVehicleFit(Vehicle vehicle, int place)
        {
            if (vehicle.Size <= ParkingList[place].AvailableSpace)
            {
                return true;
            }
            return false;
        }
        private bool DoesBigVehicleFit(Vehicle veh, int place) 
        {
            //int amountOfSpots = veh.Size / Configuration.ParkingSpotSize;
            //int count = 0;
            
            bool vehicleFits = 
            ParkingList[place].AvailableSpace == Configuration.ParkingSpotSize &&
            ParkingList[place + 1].AvailableSpace == Configuration.ParkingSpotSize && //jokes, ska fixa
            ParkingList[place + 2].AvailableSpace == Configuration.ParkingSpotSize &&
            ParkingList[place + 3].AvailableSpace == Configuration.ParkingSpotSize;
            return vehicleFits;
            //for (int i = place; i < place + amountOfSpots; i++)
            //{
            //    if (ParkingList[i].AvailableSpace == Configuration.ParkingSpotSize &&
            //        ParkingList[i + 1].AvailableSpace == Configuration.ParkingSpotSize) //något fel med denna, det går att flytta bussar till plats där det redan står fordon
            //    {
            //        count++;
            //    }
            //    else
            //    {
            //        count = 0;
            //    }
            //    if (count == amountOfSpots - 1)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// returns a vehicle object AND where its standing in the parkinglot
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns></returns>
        private (Vehicle, ParkingSpot) GetVehicleStatus(string regNum) //använder ej
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
        public bool CheckReg(string regnum)
        {
            foreach (var spots in ParkingList)
            {
                foreach (var v in spots.VehiclesParked)
                {
                    if (v.RegNumber == regnum)
                    {
                        return false;
                    }
                }
            }
            return true;
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

            for (int i = 0; i < Configuration.ParkingHouseSize; i++) //fixa detta så att det läser parkinghousesize från en fil
            {
                if (ParkingList[i].AvailableSpace == Configuration.ParkingSpotSize)
                {
                    parkingSpotColorMarking = "green";
                }
                else if (ParkingList[i].AvailableSpace > 0 || ParkingList[i].AvailableSpace < Configuration.ParkingSpotSize)
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
            List<string> prices = Configuration.GetPriceList();
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
            for (int i = 0; i < Configuration.ParkingHouseSize; i++) //läs in settings från en fil som säger att storleken på parkeringshuset ska vara 100
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });
            }
        }
        private void ExpandParkingLot() //om man i configfilen ökar antalet p-platser
        {
            for (int i = ParkingList.Count; i < Configuration.ParkingHouseSize; i++)
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });
            }
        }
    }
}
