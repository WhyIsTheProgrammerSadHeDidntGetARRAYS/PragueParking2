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
            Configurations.SetConfigValues();//TODO: handle exceptions when reading from files
            Configurations.SetPrices();
            
            Menu menu = new Menu();
            while (true)
            {
                menu.MenuChoices();
            }
        }
    }
}
