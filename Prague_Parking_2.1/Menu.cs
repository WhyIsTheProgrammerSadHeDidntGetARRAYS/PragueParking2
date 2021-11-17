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
                .PageSize(10)
                .AddChoices(new[] {
                    "Park Vehicle", "Move vehicle", "Remove Vehicle", "Search for vehicle", 
                    "Parkinglot Overview", "Print Parked Vehicles", "Our prices", "Clear ALL vehicles", "Edit settings", "Exit Program" }));

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
                    SpecifyVehicletypeToPark();
                    break;

                case "Move vehicle":
                    parkinglot.Move();
                    break;

                case "Remove Vehicle":
                    parkinglot.CheckOut();
                    break;

                case "Search for vehicle":
                    parkinglot.SearchForVehicle();
                    Console.ReadKey();
                    break;

                case "Parkinglot Overview":
                    parkinglot.ParkingLotOverview();
                    break;

                case "Print Parked Vehicles":
                    parkinglot.PrintParkedVehicles();
                    break;

                case "Our prices":
                    parkinglot.PrintPrice();
                    break;
                case "Clear ALL vehicles":
                    parkinglot.ClearAllVehicles();
                    Console.ReadKey();
                    break;

                case "Edit settings":
                    UserDialogue.ChangeSettingsOption();
                    break;

                case "Exit Program":
                    Environment.Exit(0);
                    break;
            }
        }
        public void SpecifyVehicletypeToPark()
        {
            UserDialogue.DisplayOption("PARK VEHICLE");
            string regnum = UserDialogue.AskForRegNum();
            ParkingLot pLot = new ParkingLot();
            if (Vehicle.IsValidRegNum(regnum) && pLot.DoesRegnumExist(regnum))
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Choose what type of vehicle you want to park[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Car", "Motorcycle", "Bike", "Bus", "Back to menu"}));
                switch (choice)
                {
                    case "Car":
                        Car car = new Car(regnum);
                        pLot.ParkVehicle(car);
                        break;

                    case "Motorcycle":
                        MC mc = new MC(regnum);
                        pLot.ParkVehicle(mc);
                        break;

                    case "Bike":
                        Bike bike = new Bike(regnum);
                        pLot.ParkVehicle(bike);
                        break;

                    case "Bus":
                        Bus bus = new Bus(regnum);
                        pLot.ParkVehicle(bus);
                        break;

                    case "Back to menu":
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
