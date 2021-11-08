using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Prague_Parking_2._0
{
    class Menu
    {
        private string MainMenuOptions()
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Prague Parking 2.0[/]\n\nUse [green](Arrowkeys)[/] to browse through the menu. Hit [green](Enter)[/] to pick something.")
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
                    parkinglot.CheckOutVehicle();
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
            if (ParkingLot.IsValidRegNum(regnum))
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Choose what type of vehicle you want to park[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Car", "Motorcycle"}));
                switch (choice)
                {
                    case "Car":
                        pLot.AddCar(regnum);
                        break;
                    
                    case "Motorcycle":
                        pLot.AddMC(regnum);
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
