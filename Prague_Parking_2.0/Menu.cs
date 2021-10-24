using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._0
{
    class Menu
    {
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose something from the menu below, by simply typing in the number\n" +
                    "that corresponds to your desired option, and hit 'Enter'\n" +
                                  "     \n" +
                                  "[1] Park vehicle\n" +
                                  "[2] Our prices\n" +
                                  "[3] Search vehicle");
                ParkingLot parkinglot = new ParkingLot();
                

                string userInput = Console.ReadLine().ToUpper();
                switch (userInput)
                {
                    case "1":
                        parkinglot.AddCar();
                        break;

                    case "2":
                        Configurations.GetPriceFromFile();
                        Console.ReadKey();
                        break;

                    case "3":
                        parkinglot.Search();
                        Console.ReadKey();
                        break;
                }    
            }
            
        }
        
    }
}
