using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Spectre.Console;

namespace Prague_Parking_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Prague Parking 2";
            Configurations.ReadConfigFile();//TODO: handle exceptions when reading from files
            ParkingLot lot = new ParkingLot();

            Menu menu = new Menu();
            while (true)
            {
                menu.MainMenuChoices();
            }
        }
    }
}
