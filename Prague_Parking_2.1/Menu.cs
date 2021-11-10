using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Prague_Parking_2._1
{
    class Menu
    {
        private string MainMenuOptions()
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Prague Parking 2.1[/]\n\nUse [green](Arrowkeys)[/] to browse through the menu. Hit [green](Enter)[/] to pick something.")
                .PageSize(7)
                .AddChoices(new[] {
                    "Park Vehicle", "Move vehicle", "Our prices", "Search for vehicle", "Print Vehicles", "Remove","Exit Program" }));

            return choice;
        }
        public void MainMenuChoices()
        {
            string menuChoice = MainMenuOptions();
            ParkingLot parkinglot = new ParkingLot();
            Console.Clear();

            switch (menuChoice)
            {
                case "Park Vehicle":
                    ParkVehicleType();
                    break;

                case "Move vehicle":
                    parkinglot.MoveVehicle();
                    break;

                case "Our prices":
                    parkinglot.PrintPrice();
                    break;

                case "Search for vehicle":
                    parkinglot.SearchForVehicle();
                    Console.ReadKey();
                    break;

                case "Print Vehicles":
                    parkinglot.ParkingLotOverview();
                    break;

                case "Remove":
                    parkinglot.RemoveVehicle();
                    break;

                case "Exit Program":
                    Environment.Exit(0);
                    break;
            }
        }
        public void ParkVehicleType()
        {
            UserDialogue.DisplayOption("PARK VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            ParkingLot pLot = new ParkingLot();
            if (ParkingLot.IsValidRegNum(regnum) && pLot.CheckReg(regnum))
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Choose what type of vehicle you want to park[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Car", "Motorcycle", "Bike", "Bus"}));
                switch (choice)
                {
                    case "Car":
                        Car car = new Car(regnum);
                        pLot.ParkSmallVehicle(car);
                        break;

                    case "Motorcycle":
                        MC mc = new MC(regnum);
                        pLot.ParkSmallVehicle(mc);
                        break;

                    case "Bike":
                        Bike bike = new Bike(regnum);
                        pLot.ParkSmallVehicle(bike);
                        break;

                    case "Bus":
                        Bus bus = new Bus(regnum);
                        pLot.ParkBigVehicle(bus);
                        break;
                }
            }
            else
            {
                UserDialogue.ErrorMessage();
                Console.ReadKey();
            }
        }
    }
}
