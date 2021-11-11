using System;

namespace Prague_Parking_2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            //ParkingLot lot = new ParkingLot();
            
            Console.Title = "Prague Parking 2.1";

            Menu menu = new Menu();
            while (true)
            {
                menu.MainMenuChoices();
            }
        }
    }
}
