using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Prague_Parking_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.MainMenu();
            //ParkingLot lot = new ParkingLot();
            //lot.PrintArray();
        }
    }
}
