using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Prague_Parking_2._0
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
            Console.WriteLine("Something went wrong. Please take a look at your input.");
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

            //Table newTable = new Table().Centered();
        }
    }
}
