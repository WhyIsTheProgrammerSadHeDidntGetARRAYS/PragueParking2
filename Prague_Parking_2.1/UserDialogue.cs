using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.IO;
using Newtonsoft.Json;

namespace Prague_Parking_2._1
{
    public static class UserDialogue
    {
        /// <summary>
        /// Asks user for a regnumber
        /// </summary>
        /// <returns>returns user input as a string</returns>
        public static string AskForRegNum()
        {
            Console.WriteLine("Type in your registration number. Please use uppercase for letters.");
            string regNum = Console.ReadLine();
            return regNum;
        }
        public static void ErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong. Please try again.");
            Console.ResetColor();
        }
        public static void ParkinglotFull()
        {
            Table table = new Table();
            table.AddColumn((new TableColumn("[red]The parkinglot is full![/]").Centered()).Alignment(Justify.Center));
            AnsiConsole.Write(table);
        }
        /// <summary>
        /// tells the user that everything went OK. 
        /// The string parameter is used to display "what" went well.
        /// </summary>
        /// <param name="option"></param>
        public static void SuccessMessage(string option)
        {
            Table table = new Table();
            table.AddColumn((new TableColumn($"[green]You have successfully {option} your vehicle![/]").Centered()).Alignment(Justify.Center));
            AnsiConsole.Write(table);
            Table table2 = new Table();
            table2.AddColumn((new TableColumn("[grey]Press any key to get back to the menu.[/]").Centered()).Alignment(Justify.Center));
            AnsiConsole.Write(table2);
        }
        public static void DisplayOption(string optionTable)
        {
            Console.Clear();
            Table table = new Table();
            table.AddColumn((new TableColumn($"[aqua]{optionTable}[/]").Centered()).Alignment(Justify.Center));
            AnsiConsole.Write(table);
        }
        public static void PrintVehicleFoundInfo(Vehicle vehicle)
        {
            Console.CursorVisible = false;
            Table table = new Table();
            table.AddColumn(new TableColumn("[green]Vehicle was found!\nHere is some information about it[/]"));
            AnsiConsole.Write(table);
            var table2 = new Table();
            table2.AddColumn($"Vehicletype: {vehicle.VehicleIdentifier} | Reg.Nr: {vehicle.RegNumber} | Arrival: {vehicle.CheckIn}");
            AnsiConsole.Write(table2);
        }
        public static void PrintVehicleNotFound()
        {
            Console.CursorVisible = false;
            Table table = new Table();
            table.AddColumn(new TableColumn("[red]Vehicle is NOT present in the parkinglot[/]"));
            AnsiConsole.Write(table);
        }
        public static void PrintConfirmPayment()
        {

        }
        /// <summary>
        /// Prints a table which is used when printing the parkinglot
        /// </summary>
        public static void PrintParkingLotTable()
        {
            Console.CursorVisible = false;
            var table = new Table();
            table.AddColumn(new TableColumn("[aqua]Parkinghouse overview[/]").Centered()).Alignment(Justify.Center);
            table.AddRow("[aqua]*NOTE* Each parkingspot is colormarked. The color of each parkingspot reflects the availability[/]");
            AnsiConsole.Write(table);

            Table t1 = new Table();
            t1.AddColumns("[grey]EMPTY SPOT =[/] [green]GREEN[/]", "[grey]FULL SPOT =[/] [red]RED[/]", "[grey] HALF-FULL[/] = [yellow]YELLOW[/]").Centered().Alignment(Justify.Center);
            AnsiConsole.Write(t1);

        }
        public static string ChangeSettingsChoices()
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Change settings![/]\n\nUse [green](Arrowkeys)[/] to browse through the menu. Hit [green](Enter)[/] to pick something.")
                .PageSize(4)
                .AddChoices(new[] {
                    "Change Price", "Change amount of parkingspots", "Back to menu" }));
            return choice;
        }
        public static void ChangeSettingsOption()
        {
            string choice = ChangeSettingsChoices();

            switch (choice)
            {
                case "Change Price":
                    SpecifyPriceChange();
                    break;

                case "Change amount of parkingspots":
                    ChangeParkingLotSettings();
                    break;

                case "Back to menu":
                    break;
            }
        }
        static string ChangePriceOptionsMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[grey]Changing the price![/]\n\nUse [green](Arrowkeys)[/] to browse through the menu. Hit [green](Enter)[/] to pick the\n" +
                "vehicle you'd like to change price for.")
                .PageSize(4)
                .AddChoices(new[] {
                    "Car Price", "MC Price", "Bike Price", "Bus Price" }));
            return choice;
        }

        public static void SpecifyPriceChange()
        {
            string selection = ChangePriceOptionsMenu();
            PriceConfiguration config = new PriceConfiguration();
            switch (selection)
            {
                case "Car Price":
                    int newCarPrice = GetTheNewPrice();
                    if(newCarPrice != -1)
                    {
                        config.WriteToPriceConfig("CarPricePerHour", newCarPrice);
                    }
                    break;

                case "MC Price":
                    int newMcPrice = GetTheNewPrice();
                    if(newMcPrice != -1)
                    {
                        //SetTheNewPrice("MCPricePerHour", newMcPrice);
                        config.WriteToPriceConfig("MCPricePerHour", newMcPrice);
                    }
                    break;

                case "Bike Price":
                    int newBikePrice = GetTheNewPrice();
                    if (newBikePrice != -1)
                    {
                        //SetTheNewPrice("BikePricePerHour", newBikePrice);
                        config.WriteToPriceConfig("BikePricePerHour", newBikePrice);
                    }
                    break;

                case "Bus Price":
                    int newBusPrice = GetTheNewPrice();
                    if (newBusPrice != -1)
                    {
                        //SetTheNewPrice("BusPricePerHour", newBusPrice);
                        config.WriteToPriceConfig("BusPricePerHour", newBusPrice);
                    }
                    break;

                default:
                    break;
            }
        }

        public static int GetTheNewPrice()
        {
            Console.WriteLine("Type in the new price.");

            bool valid = int.TryParse(Console.ReadLine(), out int newPrice);
            if (valid)
            {
                if (newPrice <= 0 || newPrice >= 500) //500 bara för att ha någon slags "rimlig gräns"
                {
                    return -1;
                }
            }
            return newPrice;
        }
        public static void SetTheNewPrice(string option, int price)
        {
            string path = File.ReadAllText(@"../../../Datafiles/Prices.json"); //filhanteringen ska ske i config klassen
            dynamic jsonObj = JsonConvert.DeserializeObject(path);
            jsonObj[option] = price; //detta låter mig ändra värdet direkt in i jsonfilen
            string json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"../../../Datafiles/Prices.json", json);
            SuccessMessage("changed the price for");
            Console.ReadKey();
        }

        public static void ChangeParkingLotSettings()
        {
            Console.WriteLine("You chose to change the amount of parkingspots. Please specify the amount\n" +
                "of parkingspots you would like too have.");

            int.TryParse(Console.ReadLine(), out int amountOfSpots);
            ParkingLot lot = new ParkingLot();

            if (lot.DecreaseParkingLot(amountOfSpots) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR. Some spots could not be removed as some vehicles may be standing on\n" +
                    "parkingspots you tried to remove!\n" +
                    "Please make sure to remove/move them before deleting spots");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            ParkingConfiguration config = new ParkingConfiguration();
            config.WriteToParkingConfig(amountOfSpots);
            
        }
    }
}
