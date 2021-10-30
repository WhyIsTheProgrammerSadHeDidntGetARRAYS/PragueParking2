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
        //private int SelectedIndex;
        //private string MenuMessage;
        //private string[] MenuOptions;

        //public Menu(string menuMessage, string[] menuOptions)
        //{
        //    MenuMessage = menuMessage;
        //    MenuOptions = menuOptions;
        //    SelectedIndex = 0;
        //}
        private string MainMenu()
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
        public void MenuChoices()
        {
            string menuChoice = MainMenu();
            ParkingLot parkinglot = new ParkingLot();
            Console.Clear();

            switch (menuChoice)
            {
                case "Park Vehicle":
                    parkinglot.AddMC();
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
                    parkinglot.PrintVehicles();
                    break;

                case "Remove":
                    parkinglot.Remove();
                    break;

                case "Exit Program":
                    Environment.Exit(0);
                    break;
            }
        }
        //försökte göra en meny själv, men blev mycket laggigare när man ska re-rendera/re painta "bilden"...
        //private void DisplayMenuOptions()
        //{
        //    Console.WriteLine(MenuMessage);
            
        //    for (int i = 0; i < MenuOptions.Length; i++)
        //    {
        //        string currentIndexValue = MenuOptions[i];

        //        if (SelectedIndex == i)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Black;
        //            Console.BackgroundColor = ConsoleColor.Green;
        //        }
        //        else
        //        {
        //            Console.ForegroundColor = ConsoleColor.White;
        //            Console.BackgroundColor = ConsoleColor.Black;
        //        }
        //        Console.WriteLine($">> {currentIndexValue} <<");
        //    }
        //    Console.ResetColor();
        //}
        //private int Run()
        //{
        //    ConsoleKey keyInput;
        //    do
        //    {
        //        Console.Clear();
        //        DisplayMenuOptions();
        //        Console.CursorVisible = false;

        //        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        //        keyInput = keyInfo.Key;
        //        if (keyInput == ConsoleKey.DownArrow)
        //        {
        //            SelectedIndex++;
        //            if (SelectedIndex == MenuOptions.Length)
        //            {
        //                SelectedIndex = MenuOptions.Length - 1;
        //            }
        //        }
        //        else if (keyInput == ConsoleKey.UpArrow)
        //        {
        //            SelectedIndex--;
        //            if (SelectedIndex < 0)
        //            {
        //                SelectedIndex = 0;
        //            }
        //        }


        //    } while (keyInput != ConsoleKey.Enter);

        //    Console.CursorVisible = true;
        //    return SelectedIndex;
        //}

        //public void MenuChoice()
        //{
        //    int menuChoice = Run();
        //    ParkingLot parkinglot = new ParkingLot();
        //    Console.Clear();

        //    switch (menuChoice)
        //    {
        //        case 0:
        //            parkinglot.AddCar();
        //            break;

        //        case 1:
        //            Configurations.GetPriceFromFile();
        //            Console.ReadKey();
        //            break;

        //        case 2:
        //            parkinglot.Search();
        //            Console.ReadKey();
        //            break;

        //        case 3:
        //            Environment.Exit(0);
        //            break;
        //    }

        //}
    }
}
