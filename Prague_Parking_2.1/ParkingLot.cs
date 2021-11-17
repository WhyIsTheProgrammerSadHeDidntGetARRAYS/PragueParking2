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
        ParkingConfiguration config = ParkingConfiguration.ReadParkingConfig();
        
        List<ParkingSpot> parkingList { get; set; } = ManageFileData.ReadParkinglist(); //we TRY to set the list equal to objects of parkingspots from parkingfile

        public ParkingLot()
        {
            if (parkingList == null)
            {
                parkingList = new List<ParkingSpot>(capacity: config.ParkingSpots);
                AddNewParkinglot();
            }
            if (parkingList.Count < config.ParkingSpots)
            {
                ExpandParkingLot();
            }
            if (parkingList.Count > config.ParkingSpots)
            {
                DecreaseParkingLot(config.ParkingSpots);
            }
            ManageFileData.UpdateParkingList(parkingList);
        }

        /// <summary>
        /// Find the first available slot for a small vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private ParkingSpot FindSmallVehicleSpot(Vehicle vehicle)
        {
            ParkingSpot spot = parkingList.Find(x => x.AvailableSpace >= vehicle.Size);
            return spot;
        }

        /// <summary>
        /// using a hashset to store parkingspots with enough space in sequence, to park a bigger vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>A HashSet of parkingspots</returns>
        private HashSet<ParkingSpot> FindBigVehicleSpots(Vehicle vehicle)//teoretiskt sätt hade detta funkat med de små fordonen också, om man avrundar rätt med divisionen :>
        {
            var set = new HashSet<ParkingSpot>();
            int counter = 0;
            int amountOfSpots = vehicle.Size / config.ParkingSpotSize; //ha amountOfSpots som en double

            foreach (var spot in parkingList)
            {
                if (spot.AvailableSpace == config.ParkingSpotSize)
                {
                    counter++;
                    set.Add(spot);
                }
                else
                {
                    counter = 0;
                    set.Clear();
                }
                if (counter == amountOfSpots)
                {
                    return set;
                }
            }
            return null;
        }

        public void ParkVehicle(Vehicle vehicle) //gjorde mest denna för att slippa ha utskrifter i metoden som faktiskt lägger till fordonen
        {
            if (vehicle.Size < config.BusSize)
            {
                if (ParkSmallVehicle(vehicle, out ParkingSpot spot))
                {
                    UserDialogue.SuccessMessage("parked");
                    Console.WriteLine("Your vehicle is standing at parkingwindow {0}", spot.ParkingWindow);
                    Console.ReadKey();
                    return;
                }
            }
            if (ParkBigVehicle(vehicle))
            {
                UserDialogue.SuccessMessage("parked");
                Console.ReadKey();
                return;
            }
            UserDialogue.ParkinglotFull();
            Console.ReadKey();
        }

        /// <summary>
        /// Handles the parking of a big vehicles(bus). Parks it 4 times, because thats what it takes....
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bus"></param>
        private bool ParkBigVehicle(Vehicle vehicle, int spotNumber = 0)
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
                ManageFileData.UpdateParkingList(parkingList);
                return true;
            }
            return false;
        }


        /// <summary>
        /// handles parking of all "small vehicles"
        /// </summary>
        /// <param name="vehicle"></param>
        private bool ParkSmallVehicle(Vehicle vehicle, out ParkingSpot parkingSpot, int spotNumber = 0) //frivillig parameter för att parkera på "given" plats
        {
            ParkingSpot spot = FindSmallVehicleSpot(vehicle);
            if (spot != null)
            {
                spot.AddVehicle(vehicle);
                parkingSpot = spot;
                ManageFileData.UpdateParkingList(parkingList);
                return true;
            }
            parkingSpot = null;
            return false;
        }

        /// <summary>
        /// searches for vehicles with a certain regnumber
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns>Dictionary of KEY parkingspots and VALUE vehicles</returns>
        private Dictionary<ParkingSpot, Vehicle> GetSpecificVehicle(string regNum)
        {
            var dictionary = new Dictionary<ParkingSpot, Vehicle>();
            
            foreach (var spot in parkingList)
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
        public void CheckOut()
        {
            UserDialogue.DisplayOption("CHECKOUT VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            Dictionary<ParkingSpot, Vehicle> set = GetSpecificVehicle(regnum);
            double price = 0;
            if (set.Count > 0)
            {
                foreach (var item in set)
                {
                    price = CalculatePrice(item.Value);
                    if (item.Value.VehicleIdentifier.Equals("BUS"))//kollar ifall fordonet är buss, för att manuellt ställa om availablespace
                    {
                        item.Key.RemoveVehicle(item.Value);
                        item.Key.AvailableSpace = config.ParkingSpotSize;
                        
                    }
                    else //tar bort enstaka fordon
                    {
                        item.Key.RemoveVehicle(item.Value);
                    }
                }
                Console.WriteLine("Total price to pay: {0} CZK", price);
                ManageFileData.UpdateParkingList(parkingList);
                UserDialogue.SuccessMessage("checked out");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Vehicle not found!");
            Console.ReadKey();
        }
        
        public void Move()
        {
            UserDialogue.DisplayOption("MOVE VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            Dictionary<ParkingSpot, Vehicle> set = GetSpecificVehicle(regnum);

            Console.WriteLine("Type in your desired spot to move too [1- 100]");
            bool validation = int.TryParse(Console.ReadLine(), out int parkingSpot) && set.Count > 0;
            
            if (!ValidateUserIndexInput(parkingSpot))
            {
                return;
            }

            if (validation)
            {
                parkingSpot -= 1; //to get the right index
                foreach (var item in set)
                {
                    if (item.Value.Size == config.BusSize)
                    {
                        if(DoesBusFit(item.Value, parkingSpot))
                        {
                            parkingList[parkingSpot].AddVehicle(item.Value);
                            parkingList[parkingSpot++].AvailableSpace = 0;
                            item.Key.RemoveVehicle(item.Value);
                            item.Key.AvailableSpace = config.ParkingSpotSize;
                        }
                    }
                    else if (DoesSmallVehicleFit(item.Value, parkingSpot))
                    {
                        parkingList[parkingSpot].AddVehicle(item.Value);
                        item.Key.RemoveVehicle(item.Value);
                    }
                }
                ManageFileData.UpdateParkingList(parkingList);
            }
        }
        /// <summary>
        /// hjälpmetod till flytta vehicle, som validerar userinput
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool ValidateUserIndexInput(int place)
        {
            if(place <= 0 || place > parkingList.Count - 1)
            {
                UserDialogue.ErrorMessage();
                Console.WriteLine("Value must be within the range of the amount of parkingspots!");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        /// <summary>
        /// checks if smaller vehicle fits in a given parkingspot from the user
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool DoesSmallVehicleFit(Vehicle vehicle, int place)
        {
            return vehicle.Size <= parkingList[place].AvailableSpace;
        }

        private bool DoesBusFit(Vehicle veh, int place)
        {
            int amountOfSpots = veh.Size / config.ParkingSpotSize;
            int counter = 0;
            
            for (int i = place; i < place + amountOfSpots; i++)
            {
                if (parkingList[i].AvailableSpace == config.ParkingSpotSize)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                }
            }
            return counter == amountOfSpots; //true ifall counter = amountofspots, false annars
        } 
        
        public void ClearAllVehicles()
        {
            parkingList.ForEach(x => x.VehiclesParked.Clear()); //clears all vehicles
            parkingList.Select(x => { x.AvailableSpace = config.ParkingSpotSize; return x; }).ToList(); //sets availablespace back to default
            ManageFileData.UpdateParkingList(parkingList);
            Console.WriteLine("All vehicles have successfully been removed!");
        }
        

        /// <summary>
        /// search user interaction
        /// </summary>
        public void SearchForVehicle()
        {
            UserDialogue.DisplayOption("SEARCH VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            var check = Search(regnum);
            
            foreach (var item in check)
            {
                if (item.RegNumber == regnum)
                {
                    UserDialogue.PrintVehicleFoundInfo(item);
                    return;
                }
            }
            UserDialogue.PrintVehicleNotFound();
        }

        /// <summary>
        /// search method
        /// </summary>
        /// <param name="regNumber"></param>
        /// <returns>IEnumerable</returns>
        private IEnumerable<Vehicle> Search(string regNumber) 
        {
            var search =
                from p in parkingList
                from v in p.VehiclesParked
                where v.RegNumber == regNumber
                select v;

            return search;

        }

        public double CalculatePrice(Vehicle vehicle) //lägg i vehicle
        {
            TimeSpan span = DateTime.Now - vehicle.CheckIn;
            PriceConfiguration price = PriceConfiguration.ReadPriceConfig();
            double total = 0;

            if (span.TotalMinutes <= price.FreeParkingTimeInMinutes)
            {
                return total;
            }
            //do calculations for totalminutes here, aka convert it to hours and take the total hours * vehicleprice per hour
            double totalHours = (double)span.TotalHours; //priset kan inte bli mindre än timtaxan, gör om
            total = 0;
            if (vehicle.Size == config.BusSize)
            {
                total = totalHours * price.BusPricePerHour;
            }
            else if (vehicle.Size == config.BikeSize)
            {
                total = totalHours * price.BikePricePerHour;
            }
            else if (vehicle.Size == config.McSize)
            {
                total = totalHours * price.MCPricePerHour;
            }
            else if (vehicle.Size == config.CarSize)
            {
                total = totalHours * price.CarPricePerHour;
            }
            return Math.Round(total, 2);

        }

        public bool DoesRegnumExist(string regnum)
        {
            foreach (var spots in parkingList)
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

        public List<Vehicle> GetParkedVehicles()
        {
            var list = new List<Vehicle>();
            foreach (var parkingspot in parkingList)
            {
                foreach (var v in parkingspot.VehiclesParked)
                {
                    if (v != null)
                    {
                        list.Add(v);
                    }
                }
            }
            return list;
        }

        public void PrintParkedVehicles() //parkeringshuset ska be varje "ruta" att skriva ut sig. 
        {
            var list = GetParkedVehicles();
            Console.CursorVisible = false;
            var table = new Table();
            table.AddColumn(new TableColumn("[grey]Parked Vehicles, showing vehicletype, and reg.nr[/]").Centered()).Alignment(Justify.Center);
            AnsiConsole.Write(table);
            Table newTable = new Table();
            string parkedVehicles = "";
            foreach (var vehicle in list)
            {
                parkedVehicles += vehicle.VehicleIdentifier + ":" + vehicle.RegNumber + " | ";
            }
            newTable.AddColumn(parkedVehicles);
            AnsiConsole.Write(newTable);
            Console.ReadKey();
        }

        public void ParkingLotOverview()
        {
            UserDialogue.PrintParkingLotTable();

            Table newTable = new Table().Centered();
            var parkingSpotColorMarking = "";
            var printResult = "";

            for (int i = 0; i < config.ParkingSpots; i++) 
            {
                if (parkingList[i].AvailableSpace == config.ParkingSpotSize)
                {
                    parkingSpotColorMarking = "green";
                }
                else if (parkingList[i].AvailableSpace > 0 && parkingList[i].AvailableSpace < config.ParkingSpotSize)
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
            List<string> prices = PriceConfiguration.GetPriceList();
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
                parkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });
            }
        }

        private void ExpandParkingLot() //om man i configfilen ökar antalet p-platser
        {
            for (int i = parkingList.Count; i < config.ParkingSpots; i++)
            {
                parkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });
            }
        }

        public bool DecreaseParkingLot(int index)
        {
            for (int i = parkingList.Count - 1; i >= index; i--)
            {
                if (parkingList[i].VehiclesParked.Count == 0)
                {
                    parkingList.RemoveAt(i);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
