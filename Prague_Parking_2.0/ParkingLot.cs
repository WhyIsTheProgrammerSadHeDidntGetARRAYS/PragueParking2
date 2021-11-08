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

        public ParkingLot()
        {
            if (ParkingList == null)//if list is empty, we create a new list empty list, and "draw" a new parkinglot
            {
                ParkingList = new List<ParkingSpot>(capacity: 100);
                AddNewParkinglot();
            }
            else if (ParkingList.Count < Configurations.ParkingHouseSize)
            {
                ExpandParkingLot(); //jag kanske kan göra detta till ett menyval för skojs skull(att göra pshuet större/mindre)
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

        /// <summary>
        /// add a car by giving a registration number
        /// </summary>
        /// <param name="regnum"></param>
        public void AddCar(string regnum)
        {
            Console.Clear();
            Car car = new Car(regnum);
            ParkingSpot ps = FirstAvailableSlot(car);

            if (ps != null)
            {
                //car.RegNumber = "ändrar kod till små bokstäver"; //detta ska inte gå att ändra på.. lös det i vehicle sopa
                ps.AddVehicle(car);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                UserDialogue.SuccessMessage("parked");

            }
            else
                UserDialogue.ParkinglotFull();

            Console.ReadKey();
        }
        public void AddMC(string regnum) //temporär metod för att testa lägga till MC
        {
            Console.Clear();
            MC mc = new MC(regnum);
            ParkingSpot ps = FirstAvailableSlot(mc);
            if (ps != null)
            {
                ps.AddVehicle(mc);
                ManageFileData.UpdateParkingList(ParkingList);
                Console.WriteLine("Your vehicle has been parked at parkingwindow {0}", ps.ParkingWindow);
                UserDialogue.SuccessMessage("parked");
                Console.ReadKey();
                return;
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
        public void MoveVehicle() //jobbar på det D: borde nog skaffa en extra metod för att kolla ifall fordonet får plats i ÖNSKAD ruta
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
            }
            UserDialogue.ErrorMessage();
        }
        private bool VehicleFits(Vehicle vehicle, int place)
        {
            if (vehicle.Size <= ParkingList[place - 1].AvailableSpace)
            {
                return true;
            }
            return false;
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

            for (int i = 0; i < Configurations.ParkingHouseSize; i++) //fixa detta så att det läser parkinghousesize från en fil
            {
                if (ParkingList[i].AvailableSpace == Configurations.ParkingSpotSize)
                {
                    parkingSpotColorMarking = "green";
                }
                else if (ParkingList[i].AvailableSpace == 2)
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
            for (int i = 0; i < Configurations.ParkingHouseSize; i++) //läs in settings från en fil som säger att storleken på parkeringshuset ska vara 100
            {
                //if (i >= (Configurations.ParkingHouseSize) / 2) //idé för att underlätta att lägga till bussar
                //{
                //    ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1, ParkingSpotSize = 16, AvailableSpace = 16, });
                //}
                //else
                //{
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1/*, AvailableSpace = 4*/ });
                //}

            }
        }
        private void ExpandParkingLot() //om man i configfilen ökar antalet p-platser
        {
            for (int i = ParkingList.Count; i < Configurations.ParkingHouseSize; i++)
            {
                ParkingList.Add(new ParkingSpot { ParkingWindow = i + 1 });
            }
        }
    }
}
